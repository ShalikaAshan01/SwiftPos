using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using PointOfSales.Core.Constants;
using PointOfSales.Utils;
using PointOfSales.Views.Shared;

namespace PointOfSales.Views;

public partial class MainWindow : Window
{
    private string? _permissionCode;
    private Window? _loginWindow;
    private AuthorizedUserControl _currentView;

    public MainWindow()
    {
        InitializeComponent();

        // Subscribe to auth changes
        GlobalAuthenticator.AuthChanged += OnAuthChanged;

        // Optionally you can trigger UI update here if needed:
        // UpdateAuthUi();
    }

    /// <summary>
    /// Navigate to a specific authorized view and force login.
    /// </summary>
    public void NavigateTo(AuthorizedUserControl view)
    {
        _permissionCode = view.PermissionCode;

        // Reset auth status, so login popup triggers
        var hasPermission =
            GlobalAuthenticator.UserPermissions.TryGetValue(PermissionCodes.GetPermissionId(_permissionCode),
                out var isMfa);
        if (!hasPermission || isMfa)
        {
            GlobalAuthenticator.IsAuthenticated = false;
        }


        // Update UI and show login
        UpdateAuthUi();

        // Show the requested content after successful login only
        _currentView = view;
        MainContent.Content = _currentView;
    }

    /// <summary>
    /// Update UI based on authentication status.
    /// Show login popup if not authenticated.
    /// </summary>
    private void UpdateAuthUi()
    {
        if (!GlobalAuthenticator.IsAuthenticated)
        {
            // Login window already open? No action.
            if (_loginWindow != null) return;

            ApplyBlurEffect();

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

            // Show login modally; this blocks until closed
            _loginWindow.ShowDialog(this);
            return;
        }

        // Authenticated: remove blur and close login window if open
        RemoveBlurEffect();

        if (_loginWindow != null)
        {
            // Unsubscribe event before close to avoid recursion or leaks
            _loginWindow.Close();
            // _loginWindow.Closing -= OnLoginWindowClosing;
            _loginWindow = null;
        }
    }

    /// <summary>
    /// Prevent login window closing if not authenticated.
    /// </summary>
    private void OnLoginWindowClosing(object? sender, CancelEventArgs e)
    {
        if (!GlobalAuthenticator.IsAuthenticated)
        {
            e.Cancel = true; // Block closing login window
        }
        else
        {
            _loginWindow = null;
            _currentView.Reload();
            RemoveBlurEffect();
        }
    }

    /// <summary>
    /// Called when authentication state changes.
    /// Ensures UpdateAuthUi runs on the UI thread.
    /// </summary>
    private void OnAuthChanged(bool isAuthenticated)
    {
        Dispatcher.UIThread.Post(UpdateAuthUi);
    }

    private void ApplyBlurEffect()
    {
        if (Content is Control content)
        {
            content.Effect = new BlurEffect { Radius = 5 };
        }
    }

    private void RemoveBlurEffect()
    {
        if (Content is Control content)
        {
            content.Effect = null;
        }
    }
}