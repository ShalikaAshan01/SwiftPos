namespace PointOfSales.Core.Keyboard;

public record VirtualKey(string Display, string Value, KeyTypes KeyType = KeyTypes.None)
{
    public readonly string Display = Display;
    public readonly string Value = Value;
    public readonly KeyTypes KeyType = KeyType;
}