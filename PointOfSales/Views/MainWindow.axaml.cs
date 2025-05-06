using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using PointOfSales.Utils;
using PointOfSales.Views.Shared;

namespace PointOfSales.Views;

public partial class MainWindow : Window
{
    private string? _permissionCode;
    public MainWindow()
    {
        InitializeComponent();
        GlobalAuthenticator.AuthChanged += OnAuthChanged;
        // Opened += (_, _) => UpdateAuthUi();
        // MainContent.Content = new Onboarding(); // Swap views here
    }
    
    public void NavigateTo(AuthorizedUserControl view)
    {
        // Add auth false
        _permissionCode = view.PermissionCode;
        GlobalAuthenticator.IsAuthenticated = false;
        UpdateAuthUi();
        MainContent.Content = view;
    }
    
    
    private Window? _loginWindow;

    private void UpdateAuthUi()
    {
        if (!GlobalAuthenticator.IsAuthenticated)
        {
            if (_loginWindow != null) return;
            ApplyBlurEffectToMainWindow(this);

            _loginWindow = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 300,
                Height = 450,
                CanResize = false,
                ShowInTaskbar = false,
                Content = new LoginPopUp(_permissionCode),
                ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
                ExtendClientAreaToDecorationsHint = true,
            };
            _loginWindow.Closing += OnLoginWindowClosing;

            _loginWindow.ShowDialog(this);
            return;
        }
        
        RemoveBlurEffectFromMainWindow(this);
        _loginWindow?.Close(); // 🔐 This ensures it closes from any UI
        _loginWindow = null;
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