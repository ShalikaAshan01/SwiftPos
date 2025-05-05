using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PointOfSales.Utils;

namespace PointOfSales.Views.Shared;

public partial class LoginPopUp : UserControl
{
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public LoginPopUp()
    {
        InitializeComponent();
    }
    private void Login_Click(object? sender, RoutedEventArgs e)
    {
        Utils.Common.Logger.LogInfo("Login Clicked: Username: {0}, Password:<PASSWORD>", "");
        GlobalAuthenticator.IsAuthenticated = true;
        // _authService.Login(UsernameBox.Text, PasswordBox.Text);
    }
}