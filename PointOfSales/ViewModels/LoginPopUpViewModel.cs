using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointOfSales.Core.Commands;
using PointOfSales.KeyBehaviors;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels;

public partial class LoginPopUpViewModel : ViewModelBase
{
    private string _username = "";
    private string _password = "";
    private readonly ILoginCommand _loginCommand;
    
    public string Username
    {
        get => _username;
        set => this.SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => this.SetProperty(ref _password, value);
    }
    
    public LoginPopUpViewModel(ILoginCommand loginCommand)
    {
        VirtualKeyboardHelper.SubmitTriggered += OnKeyboardSubmit;
        _loginCommand = loginCommand;
    }    
    [RelayCommand]
    private async Task Login()
    {
        Utils.Common.Logger.LogInfo("Login Clicked: Username: {0}, Password:<PASSWORD>", _username);
        await _loginCommand.CheckUserNameAsync(_username);
        GlobalAuthenticator.IsAuthenticated = true;
        VirtualKeyboardHelper.CloseKeyboard();
    }

    private void OnKeyboardSubmit(object? sender, EventArgs e)
    {
        VirtualKeyboardHelper.SubmitTriggered -= OnKeyboardSubmit;
        LoginCommand.Execute(null);
    }
    public void Dispose()
    {
        VirtualKeyboardHelper.SubmitTriggered -= OnKeyboardSubmit;
    }
}