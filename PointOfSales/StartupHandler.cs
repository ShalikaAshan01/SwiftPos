using System;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System.Threading.Tasks;
using PointOfSales.Views;
using PointOfSales.Core.Constants;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;
using PointOfSales.Engine.Utils;

namespace PointOfSales
{
    internal class StartupHandler
    {
        private readonly IApplicationLogger _logger;
        private readonly IIniEngine _iniEngine;
        // private readonly IPluginLoader _pluginLoader;
        public static IClassicDesktopStyleApplicationLifetime Desktop = null!;
        public readonly IDatabaseProvider _DatabaseProvider;

        public StartupHandler(IApplicationLogger logger, IIniEngine iniEngine, IDatabaseProvider databaseProvider)
        {
            _logger = logger;
            _iniEngine = iniEngine;
            _DatabaseProvider = databaseProvider;
        }

        public void InitUi(IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new SplashScreen();
            desktop.MainWindow.Width = LocalConfigurations.SplashScreenWidth;
            desktop.MainWindow.Height = LocalConfigurations.SplashScreenHeight;
            desktop.MainWindow.CanResize = false;
            desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            desktop.MainWindow.Title = LocalConfigurations.ApplicationName;
            desktop.MainWindow.SystemDecorations = SystemDecorations.None;
            Desktop = desktop;
        }

        public async Task<bool> Init(ServiceCollection collection)
        {
            try
            {
                if (!Directory.Exists(LocalConfigurations.LocalFolderPath))
                {
                    Directory.CreateDirectory(LocalConfigurations.LocalFolderPath);
                    _logger.LogWarning("Creating logging file...");
                }

                _logger.LogInfo("Initializing the application...");
                await _iniEngine.InitAsync();
                // var assemblies = await _pluginLoader.LoadPluginsAsync();
                // var plugins = await _pluginLoader.InjectPluginsAsync(collection, assemblies);
                // collection.AddSingleton(typeof(IEnumerable<IPlugin>),plugins);
                await _DatabaseProvider.OnInitAsync(collection);
                
                _logger.LogInfo("Applying permissions...");
                Configurations.IniEngine = _iniEngine;
                var unitOfWork = collection.BuildServiceProvider().GetRequiredService<IUnitOfWork>();
                var encryptionService = collection.BuildServiceProvider().GetRequiredService<IEncryptionService>();
                
                await AddingPermission(unitOfWork);
                await AddingUser(unitOfWork, encryptionService);
                await unitOfWork.SaveChangesAsync();
                PermissionCodes.SetPermissions(await unitOfWork.PermissionRepository.GetAllAsync());

                if (Configurations.AutoAssignNewPermissionToAdmin == true)
                {
                    await AddingRole(unitOfWork);
                    await unitOfWork.SaveChangesAsync();
                }
                await Task.Delay(Configurations.SplashScreenTime);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{0} application initialization failed", nameof(Init));
            }

            return false;
        }
        
        private async Task AddingPermission(IUnitOfWork unitOfWork)
        {
            List<Permission> permissions = PermissionCodes.GetPermissions().Select(keyValuePair => new Permission
            {
                PermissionName = keyValuePair.Key,
                PermissionCode = keyValuePair.Value,
                IsActive = true,
                CreatedBy = 0,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await unitOfWork.PermissionRepository.SaveListAsync(permissions);
        }

        private async Task AddingUser(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
        {
            var admin =await unitOfWork.UserRepository.GetByUsernameAsync("Admin");
            if (admin != null)
            {
                Configurations.AdminUserId = admin.UserId;
                return;
            }
            _logger.LogInfo("Creating admin user...");
            var password = encryptionService.EncryptPasswordAsync("Admin", out var salt);
            admin = new User
            {
                UserId = 1,
                UserName = "Admin",
                ShouldChangePassword = true,
                PasswordExpiryDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Password = password,
                Salt = salt
            };
            await unitOfWork.GroupRepository.SaveAsync(new Group
            {
                Name = "Admin",
                UserGroups = [
                new UserGroup
                {
                    User = admin
                }
                ]
            });
        }

        private async Task AddingRole(IUnitOfWork unitOfWork)
        {
            var admin = await unitOfWork.GroupRepository.GetByNameAsync("Admin");
            if (admin == null)
            {
                _logger.LogInfo("The admin group is null and cannot be assigned to any role.");
                return;
            }

            var availablePermissionCodes = admin.GroupPermissions.Select(s => s.Permission.PermissionCode).ToHashSet();
            var toAdd = (from permission in PermissionCodes.Permissions where !availablePermissionCodes.Contains(permission.Key) select new GroupPermission { PermissionId = permission.Value.PermissionId, GroupId = admin.GroupId, CreatedBy = Configurations.AdminUserId }).ToList();
            await unitOfWork.GroupPermissionRepository.SaveListAsync(toAdd);
        }
    }
}
