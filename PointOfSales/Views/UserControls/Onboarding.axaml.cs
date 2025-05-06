using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PointOfSales.Utils;

namespace PointOfSales.Views.UserControls;

public partial class Onboarding : AuthorizedUserControl
{
    public Onboarding()
    {
        InitializeComponent();
    }

    public override string PermissionCode => string.Empty;
}