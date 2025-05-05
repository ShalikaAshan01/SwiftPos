using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using PointOfSales.Views.Shared;

namespace PointOfSales.Utils;

public abstract class BaseWindow: Window
{
    private Window? _loginWindow;
    protected abstract string PermissionCode { get; }

    protected BaseWindow()
    {
        // _authService = App.Current.Services.GetRequiredService<IAuthService>();
        // _authService.AuthChanged += OnAuthChanged;
        GlobalAuthenticator.AuthChanged += OnAuthChanged;
        Opened += (_, _) => UpdateAuthUi();
    }

    private void UpdateAuthUi()
    {
        // LoginOverlay.IsVisible = !_authService.IsAuthenticated;
        // ContentArea.IsEnabled = _authService.IsAuthenticated;
        // LoginOverlay.IsVisible = true;
        if (!GlobalAuthenticator.IsAuthenticated)
        {
            if (_loginWindow != null) return;
            ApplyBlurEffectToMainWindow(this);

            _loginWindow = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 400,
                Height = 300,
                CanResize = false,
                ShowInTaskbar = false,
                Content = new LoginPopUp(),
                // ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
                // ExtendClientAreaToDecorationsHint = true,
            };
            _loginWindow.Closing += OnLoginWindowClosing;

            _loginWindow.ShowDialog(this);
        }
        else
        {
            RemoveBlurEffectFromMainWindow(this);
            _loginWindow?.Close(); // 🔐 This ensures it closes from any UI
            _loginWindow = null;
        }
        
    }
    
    
    private void OnLoginWindowClosing(object? sender, CancelEventArgs e)
    {
        if (!GlobalAuthenticator.IsAuthenticated)
        {
            e.Cancel = true;
            return;
        }

        _loginWindow = null;
        RemoveBlurEffectFromMainWindow(this);
    }
    
    private void OnAuthChanged(bool isAuthenticated)
    {
        Dispatcher.UIThread.Post(UpdateAuthUi); // Ensure runs on UI thread
    }
    private void ApplyBlurEffectToMainWindow(Window mainWindow)
    {
        if (mainWindow.Content is Control content)
        {
            content.Effect = new BlurEffect
            {
                Radius = 5
            };
        }
    }

    private void RemoveBlurEffectFromMainWindow(Window mainWindow)
    {
        if (mainWindow.Content is Control content)
        {
            content.Effect = null;
        }
    }
    
}