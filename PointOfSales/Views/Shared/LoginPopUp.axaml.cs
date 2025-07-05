using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.KeyBehaviors;
using PointOfSales.Utils;
using PointOfSales.ViewModels;

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
        InitializeComponent();
        this.AttachedToVisualTree += (_, __) =>
        {
            UsernameBox.Focus();
        };
    }
    public LoginPopUp(string? permissionCode)
    {
        InitializeComponent();
        _permissionCode = permissionCode;
    }

    private void OnPasswordKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        if (DataContext is LoginPopUpViewModel vm)
        {
            vm.LoginCommand.Execute(null);
        }
        e.Handled = true;
    }
}