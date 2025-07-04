namespace PointOfSales.Core.Keyboard;

public record VirtualKey
{
    public (string Display, string Value) Normal { get; init; }
    public (string Display, string Value) Caps { get; init; }
    public double Size { get; init; } = 1;
    public KeyTypes KeyType { get; init; } = KeyTypes.Normal;

    // Constructor: Normal and Caps tuples, optional size and keytype
    public VirtualKey(
        (string Display, string Value) normal,
        (string Display, string Value) caps,
        double size = 1,
        KeyTypes keyType = KeyTypes.Normal)
    {
        Normal = normal;
        Caps = caps;
        Size = size;
        KeyType = keyType;
    }
}