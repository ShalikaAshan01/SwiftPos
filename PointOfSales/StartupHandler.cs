using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System.Threading.Tasks;
using PointOfSales.Views;
using PointOfSales.Core.Constants;
using System.IO;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;

namespace PointOfSales
{
    internal class StartupHandler
    {
        private readonly IApplicationLogger _logger;
        private readonly IIniEngine _iniEngine;
        public static IClassicDesktopStyleApplicationLifetime Desktop = null!;
        //private ApplicationLoaderViewModel viewModel = new ApplicationLoaderViewModel();
        public StartupHandler(IClassicDesktopStyleApplicationLifetime desktop, IApplicationLogger logger, IIniEngine iniEngine)
        {
            desktop.MainWindow = new SplashScreen();
            desktop.MainWindow.Width = LocalConfigurations.SplashScreenWidth;
            desktop.MainWindow.Height = LocalConfigurations.SplashScreenHeight;
            desktop.MainWindow.CanResize = false;
            //desktop.MainWindow.DataContext = viewModel;
            UpdateLoadingText("Loading default configurations...");
            desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            desktop.MainWindow.Title = LocalConfigurations.ApplicationName;
            desktop.MainWindow.SystemDecorations = SystemDecorations.None;
            Desktop = desktop;
            _logger = logger;
            _iniEngine = iniEngine;
        }

        public async Task<bool> Init()
        {
            if(!Directory.Exists(LocalConfigurations.LocalFolderPath))
            {
                Directory.CreateDirectory(LocalConfigurations.LocalFolderPath);
                _logger.LogWarning("Creating logging file...");
            }
            _logger.LogInfo("Initializing the application...");
            
            
            await Task.Delay(1000);
            return true;
        }

        private void UpdateLoadingText(string text)
        {
            //viewModel.LoadingText = text;
        }
    }
}
