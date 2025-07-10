using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Media;

namespace PointOfSales.ViewModels;

public class SideBarItemModel(string icon, string title, UserControl content,bool isActive = false, int notificationCount = 0)
    : INotifyPropertyChanged
{
    public string Icon { get; } = icon;
    public string Title { get; } = title;
    public UserControl Content { get; } = content;

    private bool _isActive = isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TitleFontWeight));
            }
        }
    }

    private int _notificationCount = notificationCount;
    public int NotificationCount
    {
        get => _notificationCount;
        set
        {
            if (_notificationCount != value)
            {
                _notificationCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNotification));
            }
        }
    }

    public bool HasNotification => NotificationCount > 0;

    public FontWeight TitleFontWeight => IsActive ? FontWeight.Bold : FontWeight.Normal;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}