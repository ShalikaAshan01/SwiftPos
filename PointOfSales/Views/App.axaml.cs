using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.Constants;
using PointOfSales.Engine;
using PointOfSales.Utils;
using System.Threading.Tasks;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;

namespace PointOfSales.Views
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                var collection = new ServiceCollection();
                collection.AddCommonServices();
                var handler = collection.BuildServiceProvider().GetRequiredService<StartupHandler>();
                handler.InitUi(desktop);
                var initStatus = await handler.Init(collection);
                if (!initStatus)
                {
                    desktop.Shutdown();
                }
                var window = new LandingScreen();
                window.Width = LocalConfigurations.MainScreenWidth;
                window.Height = LocalConfigurations.MainScreenHeight;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = LocalConfigurations.ApplicationName;
                window.SystemDecorations = SystemDecorations.None;
                desktop.MainWindow?.Hide();
                desktop.MainWindow = window;
                window.Show();

            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}