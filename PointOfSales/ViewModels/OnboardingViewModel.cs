using System;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Engine.Utils;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels;

public class OnboardingViewModel : ViewModelBase, IDisposable
{
    public OnboardingViewModel()
    {
        GlobalAuthenticator.AuthChangedUser += OnAuthChanged;
        GlobalAuthenticator.OnChangedCompany += OnCompanyNameChanged;
        GlobalAuthenticator.OnChangedLocation += OnLocationChanged;
        CanInvoice = false;
        CanBackOffice = Configurations.EnableBackOffice;
    }

    private void OnLocationChanged(Location location)
    {
        LocationCode = location.LocationCode;
        Address = location.GetFullAddress();
    }

    private string _displayName = string.Empty;
    private string _companyName = string.Empty;
    private string _address = string.Empty;
    private string _locationCode = string.Empty;
    private bool _canInvoice = false;
    private bool _canBackOffice = false;
    private bool _canStartShift = false;
    private bool _canEndShift = false;
    

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

    public bool CanInvoice
    {
        get => _canInvoice;
        set => SetProperty(ref _canInvoice, value);
    }

    public bool CanBackOffice
    {
        get => _canBackOffice;
        set => SetProperty(ref _canBackOffice, value);
    }

    public bool CanStartShift
    {
        get => _canStartShift;
        set => SetProperty(ref _canStartShift, value);
    }
    public bool CanEndShift
    {
        get => _canEndShift;
        set => SetProperty(ref _canEndShift, value);
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