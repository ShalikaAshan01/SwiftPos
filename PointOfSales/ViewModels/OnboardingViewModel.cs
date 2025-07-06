using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels;

public class OnboardingViewModel : ViewModelBase, IDisposable
{
    public OnboardingViewModel()
    {
        GlobalAuthenticator.AuthChangedUser += OnAuthChanged;
        GlobalAuthenticator.OnChangedCompany += OnCompanyNameChanged;
        GlobalAuthenticator.OnChangedLocation += OnLocationChanged;
    }

    private void OnLocationChanged(Location location)
    {
        LocationCode = location.LocationCode;
        Address = location.GetFullAddress();
    }

    private string _displayName = string.Empty;
    private string _companyName = string.Empty;
    public string _address = string.Empty;
    public string _locationCode = string.Empty;

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    public string CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
    }
    
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
    public string LocationCode
    {
        get => _locationCode;
        set => SetProperty(ref _locationCode, value);
    }

    private void OnAuthChanged(User? user)
    {
        if (user != null)
        {
            DisplayName = user.UserName;
        }
    }

    private void OnCompanyNameChanged(Company? company)
    {
        if (company != null)
        {
            CompanyName = company.CompanyName;
        }
    }

    public void Dispose()
    {
        GlobalAuthenticator.AuthChangedUser -= OnAuthChanged;
        GlobalAuthenticator.OnChangedCompany -= OnCompanyNameChanged;
    }
}