using Avalonia.Controls;

namespace PointOfSales.Utils;

public abstract class AuthorizedUserControl : UserControl
{
    public abstract string PermissionCode { get; }
}