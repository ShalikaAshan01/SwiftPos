using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels;

public class OnboardingViewModel : ViewModelBase
{
    public OnboardingViewModel()
    {
        GlobalAuthenticator.AuthChangedUser += OnAuthChanged;
        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        timer.Tick += (_, _) =>
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        };
        timer.Start();
    }
    
    public string CurrentTime { get => _currentTime; set => SetProperty(ref _currentTime, value); }
    private string _currentTime = DateTime.Now.ToString("HH:mm:ss");


    private string _displayName = string.Empty;

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    private void OnAuthChanged(User? user)
    {
        if (user != null)
        {
            UpdateUserInfo(user);
        }
    }

    private void UpdateUserInfo(User user)
    {
        DisplayName = user.UserName;
        Engine.Utils.Common.Logger.LogInfo($"User loaded: {DisplayName}");
    }
}