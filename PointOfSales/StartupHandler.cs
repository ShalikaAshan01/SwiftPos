using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System.Threading.Tasks;
using PointOfSales.Views;
using PointOfSales.Core.Constants;
using PointOfSales.Engine;
using System;
using System.IO;

namespace PointOfSales
{
    internal class StartupHandler
    {
        public IApplicationLogger logger { get; set; }
        public static IClassicDesktopStyleApplicationLifetime Desktop = null!;
        //private ApplicationLoaderViewModel viewModel = new ApplicationLoaderViewModel();
        public StartupHandler(IClassicDesktopStyleApplicationLifetime desktop, IApplicationLogger logger)
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
            this.logger = logger;
        }

        public async Task<bool> Init()
        {

            var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if(!Directory.Exists(LocalConfigurations.LocalFolderPath))
            {
                Directory.CreateDirectory(LocalConfigurations.LocalFolderPath);
                logger.LogWarning("Creating logging file...");

                //return false;
            }
            logger.LogInfo("Starting application...");

            await Task.Delay(1000);
            return true;
        }

        private void UpdateLoadingText(string text)
        {
            //viewModel.LoadingText = text;
        }
    }
}
