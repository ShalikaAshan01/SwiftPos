using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PointOfSales.Core.Constants;
using PointOfSales.Utils;

namespace PointOfSales.Views.UserControls.BackOffice;

public partial class BackOffice : AuthorizedUserControl
{
    public BackOffice()
    {
        InitializeComponent();
    }

    public override string PermissionCode => PermissionCodes.AccessBackOffice;
}