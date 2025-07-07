using Avalonia.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Utils;
using Location = PointOfSales.Core.Entities.Infrastructure.Location;

namespace PointOfSales.Views.Shared;

public partial class LoggedUserCard : UserControl, INotifyPropertyChanged
{
    private string _fullName = "Unknown";
    private string _location = "Unknown";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FullName
    {
        get => _fullName;
        set
        {
            if (_fullName != value)
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }
    }

    public string Location
    {
        get => _location;
        set
        {
            if (_location != value)
            {
                _location = value;
                OnPropertyChanged();
            }
        }
    }

    public LoggedUserCard()
    {
        InitializeComponent();
        DataContext = this;

        // Subscribe to authentication changes
        GlobalAuthenticator.AuthChangedUser += OnAuthChanged;
        GlobalAuthenticator.OnChangedLocation += OnChangedLocation;

        // Load initial user info if already logged in
        if (GlobalAuthenticator.CurrentUser is { } user)
        {
            UpdateUser(user);
        }

        if (GlobalAuthenticator.CurrentLocation != null)
            OnChangedLocation(GlobalAuthenticator.CurrentLocation);
    }

    private void OnChangedLocation(Location obj)
    {
        Location = $"{obj.LocationName}({obj.LocationCode})";
    }

    private void OnAuthChanged(User user)
    {
        UpdateUser(user);
    }

    private void UpdateUser(User user)
    {
        FullName = user.UserName;
        UserIconControl.FullName = user.UserName;
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}