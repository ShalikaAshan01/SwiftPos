using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using PointOfSales.Core.Utils;
using PointOfSales.Utils;
using PointOfSales.Views.Shared;

namespace PointOfSales.Views;

public partial class LandingScreen : BaseWindow
{
    public LandingScreen()
    {
        InitializeComponent();
    }

    protected override string PermissionCode => "";
}