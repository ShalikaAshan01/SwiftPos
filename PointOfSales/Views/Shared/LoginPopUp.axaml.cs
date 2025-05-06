using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PointOfSales.Utils;

namespace PointOfSales.Views.Shared;

public partial class LoginPopUp : UserControl
{
    private string? _permissionCode;
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public LoginPopUp()
    {
        this.AttachedToVisualTree += (_, __) =>
        {
            UsernameBox.Focus();
        };
        InitializeComponent();
        

    }
    public LoginPopUp(string? permissionCode)
    {
        InitializeComponent();
        _permissionCode = permissionCode;
    }
    private void Login_Click(object? sender, RoutedEventArgs e)
    {
        Utils.Common.Logger.LogInfo("Login Clicked: Username: {0}, Password:<PASSWORD>", "");
        GlobalAuthenticator.IsAuthenticated = true;
        // _authService.Login(UsernameBox.Text, PasswordBox.Text);
    }
    
    private void UsernameBox_Loaded(object? sender, RoutedEventArgs e)
    {
        if (sender is Control control)
        {
            control.Focus();
            OpenVirtualKeyboard();
        }
    }
    
    
    
    private void OpenVirtualKeyboard()
    {
        return;
        var virtualKeyboard = new VirtualKeyboard();
        virtualKeyboard.KeyPressed += OnKeyPressed;
        virtualKeyboard.Show();
        virtualKeyboard.Closing += OnKeyBoardClosing;

    }
    
    
    private void OnKeyPressed(object? sender, string key)
    {
        return;
        var s = this.FindControl<TextBox>("UsernameBox"); // Bind to your TextBox here

        s.Text += key;
    }
    
    private void OnKeyBoardClosing(object? sender, CancelEventArgs e)
    {
        // virtualKeyboard.KeyPressed -= OnKeyPressed;  // Unsubscribe from the event
    }
    
    
    //
    // private void MoveFocusOnEnter(object? sender, KeyEventArgs e)
    // {
    //     if (e.Key == Key.Enter && sender is IInputElement current)
    //     {
    //         var next = KeyboardNavigationHandler.GetNext(current, NavigationDirection.Next);
    //         next?.Focus();
    //         e.Handled = true;
    //     }
    // }
}