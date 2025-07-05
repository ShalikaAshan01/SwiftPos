using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PointOfSales.Utils.Converters;

public class StringNotEmptyToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => !string.IsNullOrWhiteSpace(value?.ToString());

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}