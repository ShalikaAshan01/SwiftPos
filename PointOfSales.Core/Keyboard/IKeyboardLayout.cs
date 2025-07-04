namespace PointOfSales.Core.Keyboard;

public interface IKeyboardLayout
{
    List<List<VirtualKey>> GetLayout();
    
    (byte, byte, byte) GetBackgroundColor();
    (byte, byte, byte) GetKeyBackgroundColor();
    (byte, byte, byte) GetKeyBorderColor();
}