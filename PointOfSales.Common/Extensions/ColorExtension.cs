using Avalonia.Media;

namespace PointOfSales.Common.Extensions;

public static class ColorExtension
{
    public static SolidColorBrush ToSolidColorBrush(this Color color)
    {
        return new SolidColorBrush(color);
    }
}