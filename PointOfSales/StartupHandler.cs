using System;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System.Threading.Tasks;
using PointOfSales.Views;
using PointOfSales.Core.Constants;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Plugin;
using PointOfSales.Core.Utils;
using PointOfSales.Utils;

namespace PointOfSales
{
    internal class StartupHandler
    {
        private readonly IApplicationLogger _logger;
        private readonly IIniEngine _iniEngine;
        private readonly IPluginLoader _pluginLoader;
        public static IClassicDesktopStyleApplicationLifetime Desktop = null!;

        public StartupHandler(IClassicDesktopStyleApplicationLifetime desktop, IApplicationLogger logger, IIniEngine iniEngine, IPluginLoader pluginLoader)
        {
            desktop.MainWindow = new SplashScreen();
            desktop.MainWindow.Width = LocalConfigurations.SplashScreenWidth;
            desktop.MainWindow.Height = LocalConfigurations.SplashScreenHeight;
            desktop.MainWindow.CanResize = false;
            desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            desktop.MainWindow.Title = LocalConfigurations.ApplicationName;
            desktop.MainWindow.SystemDecorations = SystemDecorations.None;
            Desktop = desktop;
            _logger = logger;
            _iniEngine = iniEngine;
            _pluginLoader = pluginLoader;
        }

        public async Task<bool> Init(ServiceCollection collection)
        {
            if(!Directory.Exists(LocalConfigurations.LocalFolderPath))
            {
                Directory.CreateDirectory(LocalConfigurations.LocalFolderPath);
                _logger.LogWarning("Creating logging file...");
            }
            _logger.LogInfo("Initializing the application...");

            var assemblies = await _pluginLoader.LoadPluginsAsync();
            var plugins = await _pluginLoader.InjectPluginsAsync(assemblies);
            collection.AddSingleton(typeof(IEnumerable<IPlugin>),plugins);

            await Task.Delay(1000);
            return true;
        }
    }
}
