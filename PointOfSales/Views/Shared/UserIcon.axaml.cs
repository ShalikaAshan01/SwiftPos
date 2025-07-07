using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PointOfSales.Views.Shared;

public partial class UserIcon : UserControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<string> FullNameProperty =
        AvaloniaProperty.Register<UserIcon, string>(nameof(FullName));

    public static readonly StyledProperty<bool> RandomizeProperty =
        AvaloniaProperty.Register<UserIcon, bool>(nameof(Randomize), defaultValue: false);

    public event PropertyChangedEventHandler? PropertyChanged;

    private static readonly Color[] ColorPalette = new[]
    {
        Color.Parse("#F87171"), // Red
        Color.Parse("#FBBF24"), // Amber
        Color.Parse("#34D399"), // Green
        Color.Parse("#60A5FA"), // Blue
        Color.Parse("#A78BFA"), // Purple
        Color.Parse("#F472B6"), // Pink
        Color.Parse("#FCD34D"), // Yellow
        Color.Parse("#4ADE80"), // Light Green
        Color.Parse("#38BDF8"), // Sky Blue
        Color.Parse("#C084FC")  // Violet
    };

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

    private string _initials = "";
    public string Initials
    {
        get => _initials;
        private set
        {
            if (_initials != value)
            {
                _initials = value;
                OnPropertyChanged();
            }
        }
    }

    public string FullName
    {
        get => GetValue(FullNameProperty);
        set => SetValue(FullNameProperty, value);
    }

    public bool Randomize
    {
        get => GetValue(RandomizeProperty);
        set => SetValue(RandomizeProperty, value);
    }

    public UserIcon()
    {
        InitializeComponent();
        DataContext = this;
        UpdateInitials();
        UpdateBackground();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == FullNameProperty)
        {
            UpdateInitials();
            UpdateBackground();
        }
        else if (change.Property == RandomizeProperty)
        {
            UpdateBackground();
        }
    }

    private void UpdateInitials()
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            Initials = "";
            return;
        }

        var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var part in parts)
        {
            if (!string.IsNullOrWhiteSpace(part))
                sb.Append(char.ToUpper(part[0]));
        }

        Initials = sb.ToString();
    }

    private void UpdateBackground()
    {
        if (!Randomize)
        {
            BackgroundBrush = Brushes.Gray;
            return;
        }

        var rand = new Random(FullName?.GetHashCode() ?? Environment.TickCount);
        var color = ColorPalette[rand.Next(ColorPalette.Length)];
        BackgroundBrush = new SolidColorBrush(color);
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
