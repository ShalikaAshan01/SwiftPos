using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
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
            OnPropertyChanged(nameof(NotificationBadgeBackgroundBrush));
            OnPropertyChanged(nameof(NotificationBadgeForegroundBrush));
        }
    }

    public bool HasNotification => NotificationCount > 0;

    public FontWeight TitleFontWeight => IsActive ? FontWeight.Bold : FontWeight.Normal;

    // Return resource keys for DynamicResource in XAML
    public string BackgroundBrush => IsActive
        ? "ThemeControlLowBrush"   // Light gray card background in FluentTheme
        : "Transparent";

    public string IconForeground => IsActive
        ? "ThemeForegroundBrush"   // Use main foreground color when active
        : "ThemeForegroundLowBrush"; // Use dimmer foreground color when inactive

    public string TextForeground => IsActive
        ? "ThemeForegroundBrush"
        : "ThemeForegroundLowBrush";

    public string NotificationBadgeBackgroundBrush => "ThemeAccentBrush"; // Accent color

    public string NotificationBadgeForegroundBrush => "SystemBaseHighColorBrush"; // Usually white in Fluent

    public SideBarItem()
    {
        InitializeComponent();
        DataContext = this;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
