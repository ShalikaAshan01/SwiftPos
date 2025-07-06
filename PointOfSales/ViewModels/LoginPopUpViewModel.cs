using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PointOfSales.Common.Resources;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.Exceptions;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;
using PointOfSales.Engine.Utils;
using PointOfSales.KeyBehaviors;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels
{
    public class LoginPopUpViewModel : ViewModelBase, IDisposable
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public LoginPopUpViewModel()
        {
            VirtualKeyboardHelper.SubmitTriggered += OnKeyboardSubmit;
            LoginCommand = new AsyncRelayCommand(Login);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public IAsyncCommand LoginCommand { get; }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = ApplicationErrors.UsernameRequired;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = ApplicationErrors.PasswordRequired;
                return;
            }

            try
            {
                if (Configurations.StoreId == short.MinValue)
                {
                    throw new SwiftException(ApplicationErrors.LocationNotAvailable, "");
                }

                //check the location
                var infrastructureEngine = GetEngine<IInfrastructureEngine>();
                var location = await infrastructureEngine.GetLocationById(Configurations.StoreId);
                if (location == null || location.IsDeleted)
                {
                    throw new SwiftException(ApplicationErrors.LocationNotAvailable, Configurations.StoreId);
                }

                if (!location.IsActive)
                {
                    throw new SwiftException(ApplicationErrors.LocationNotActive, location.LocationName);
                }

                if (!location.CompanyId.HasValue)
                {
                    throw new SwiftException(ApplicationErrors.CompanyIdNull);
                }
                var company = await infrastructureEngine.GetCompanyById(location.CompanyId.Value);
                if (company == null || company.IsDeleted)
                {
                    throw new SwiftException(ApplicationErrors.CompanyNotFound, location.CompanyId.Value);
                }

                if (!company.IsActive)
                {
                    throw new SwiftException(ApplicationErrors.CompanyNotActive, company.CompanyName);
                }

                var device = await HandleDevice();
                if (device == null)
                {
                    throw new SwiftException(ApplicationErrors.DeviceNotFound);
                }

                var permission = PermissionCodes.GetPermissionId(PermissionCodes.LoginToSystem);
                var engine = GetEngine<IAuthenticationEngine>();
                var user = await engine.AuthenticateUserAsync(_username, _password);
                var activityLog = new ActivityLog
                {
                    PermissionId = permission,
                    LocationId = Configurations.StoreId,
                    DeviceId = Configurations.MachineId,
                    Message = $"User {Username} attempt logged  in",
                    AccessedAt = DateTime.UtcNow,
                    IsSuccess = true,
                    UserId = user.UserId
                };
                await GetEngine<IUnitOfWork>().AuditLogRepository.WriteToLogAsync(activityLog);
                device.LastActiveTime = DateTime.UtcNow;

                await GetEngine<IUnitOfWork>().SaveChangesAsync();
                GlobalAuthenticator.Authenticate(user, new Company(), location, device);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                Engine.Utils.Common.Logger.LogError(e, e.ToString());
            }

            VirtualKeyboardHelper.CloseKeyboard();
        }

        private void OnKeyboardSubmit(object? sender, EventArgs e)
        {
            if (LoginCommand.CanExecute(null))
                LoginCommand.Execute(null);
        }

        public void Dispose()
        {
            VirtualKeyboardHelper.SubmitTriggered -= OnKeyboardSubmit;
        }

        private async Task<Device?> HandleDevice()
        {
            Device? device = null;
            string machineUniqueCode = GetEngine<ISystemInformation>().GetMachineUniqueCode();
            var deviceEngine = GetEngine<IInfrastructureEngine>();
            bool rewriteIni = false;
            if (Configurations.MachineId == short.MinValue)
            {
                //register the machine
                await deviceEngine.RegisterDeviceAsync(machineUniqueCode, Configurations.StoreId);
                rewriteIni = true;
            }
            else
            {
                //validate the machine ids
                device = await deviceEngine.GetDeviceByUniqueCode(machineUniqueCode);
                if (device == null || device.IsDeleted)
                {
                    throw new SwiftException(ApplicationErrors.DeviceNotFound, machineUniqueCode);
                }

                if (!device.IsActive)
                {
                    throw new SwiftException(ApplicationErrors.DeviceNotActive, machineUniqueCode);
                }

                if (device.DeviceId != Configurations.MachineId)
                {
                    throw new SwiftException(ApplicationErrors.DeviceMismatch, machineUniqueCode);
                }
            }

            //save in ini file
            if (rewriteIni)
            {
                await GetEngine<IUnitOfWork>().SaveChangesAsync();
                device = await deviceEngine.GetDeviceByUniqueCode(machineUniqueCode);
                var deviceId = device?.DeviceId ?? short.MinValue;
                var iniEngine = GetEngine<IIniEngine>();
                await iniEngine.UpdateAsync(nameof(Configurations.MachineId), deviceId);
                await iniEngine.InitAsync();
                Configurations.IniEngine = iniEngine;
                throw new SwiftException(ApplicationErrors.DeviceNotActive, machineUniqueCode);
            }

            return device;
        }
    }
}