using System;
using System.IO;
using Avalonia.Media.Imaging;
using PointOfSales.Core.Constants;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels;

public class OnboardingViewModel : ViewModelBase
{
    public OnboardingViewModel()
    {
        DisplayName = GlobalAuthenticator.CurrentUser?.UserName ?? string.Empty;
    }
    private string _displayName = string.Empty;
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }
}