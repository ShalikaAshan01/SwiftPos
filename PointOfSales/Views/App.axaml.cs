using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.Constants;
using PointOfSales.Utils;
using PointOfSales.Core.Utils;
using PointOfSales.Views.UserControls;

namespace PointOfSales.Views
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static MainWindow? MainWindowInstance { get; private set; }
        public override void Initialize()
        {
            // Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            // Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
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
                Engine.Utils.Common.Logger = collection.BuildServiceProvider().GetRequiredService<IApplicationLogger>();
                ServiceProvider = collection.BuildServiceProvider();
                MainWindowInstance = new MainWindow();
                MainWindowInstance.Width = LocalConfigurations.MainScreenWidth;
                MainWindowInstance.Height = LocalConfigurations.MainScreenHeight;
                MainWindowInstance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                MainWindowInstance.Title = LocalConfigurations.ApplicationName;
                MainWindowInstance.SystemDecorations = SystemDecorations.None;
                desktop.MainWindow?.Hide();
                desktop.MainWindow = MainWindowInstance;
                MainWindowInstance.Show();
                MainWindowInstance.NavigateTo(new Onboarding());
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}