using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace PointOfSales.Views.UserControls.BackOffice;

public partial class SideBarItem : UserControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<SideBarItem, string>(nameof(Icon));

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<SideBarItem, string>(nameof(Title));

    public static readonly StyledProperty<int> NotificationCountProperty =
        AvaloniaProperty.Register<SideBarItem, int>(nameof(NotificationCount));

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<SideBarItem, bool>(nameof(IsActive));

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public int NotificationCount
    {
        get => GetValue(NotificationCountProperty);
        set
        {
            SetValue(NotificationCountProperty, value);
            OnPropertyChanged(nameof(HasNotification));
        }
    }

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set
        {
            SetValue(IsActiveProperty, value);
            OnPropertyChanged(nameof(TitleFontWeight));
            OnPropertyChanged(nameof(BackgroundBrush));
            OnPropertyChanged(nameof(IconForeground));
            OnPropertyChanged(nameof(TextForeground));
        }
    }

    // Helper properties for UI binding
    public bool HasNotification => NotificationCount > 0;

    public FontWeight TitleFontWeight => IsActive ? FontWeight.Bold : FontWeight.Normal;

    public IBrush BackgroundBrush => IsActive
        ? new SolidColorBrush(Color.Parse("#e5e5e5")) // light gray background for active (you can adjust if needed)
        : Brushes.Transparent;

    public IBrush IconForeground => IsActive
        ? new SolidColorBrush(Color.Parse("#5c5c5c")) // darker gray for active
        : new SolidColorBrush(Color.Parse("#c6c6c6")); // lighter gray for inactive

    public IBrush TextForeground => IsActive
        ? new SolidColorBrush(Color.Parse("#5c5c5c")) // darker gray for active
        : new SolidColorBrush(Color.Parse("#c6c6c6")); //

    public SideBarItem()
    {
        InitializeComponent();
        DataContext = this;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}