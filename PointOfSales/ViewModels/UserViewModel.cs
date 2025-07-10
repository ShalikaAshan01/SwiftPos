using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;

namespace PointOfSales.ViewModels;

public class UserViewModel: ViewModelBase
{
    private string _fullName = "Unknown";
    private string _role = "Unknown";
    private string _initials = "";
    private IBrush _backgroundBrush = Brushes.Gray;
    public IBrush BackgroundBrush
    {
        get => _backgroundBrush;
        private set
        {
            if (_backgroundBrush != value)
            {
                _backgroundBrush = value;
                OnPropertyChanged();
            }
        }
    }
    public string FullName
    {
        get => _fullName;
        set
        {
            if (_fullName != value)
            {
                Initials = FullName.Length.ToString();
                _fullName = value;
                OnPropertyChanged();
            }
        }
    }

    public string Role
    {
        get => _role;
        set
        {
            if (_role != value)
            {
                _role = value;
                OnPropertyChanged();
            }
        }
    }

    public string Initials
    {
        get => _initials;
        set
        {
            if (_initials != value)
            {
                _initials = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}