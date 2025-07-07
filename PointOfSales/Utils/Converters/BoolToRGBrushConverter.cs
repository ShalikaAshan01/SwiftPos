namespace PointOfSales.Utils.Converters;

using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

public class BoolToRgBrushConverter : IValueConverter
{
    public static readonly BoolToRgBrushConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && b
            ? new SolidColorBrush(Color.Parse("#10B981")) // green
            : new SolidColorBrush(Color.Parse("#EF4444")); // red
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
