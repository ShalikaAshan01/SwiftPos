using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PointOfSales.Utils;
using PointOfSales.ViewModels;

namespace PointOfSales.Views.UserControls;

public partial class Onboarding : AuthorizedUserControl
{
    public Onboarding()
    {
        InitializeComponent();
        DataContext = new OnboardingViewModel();
    }

    public override string PermissionCode => string.Empty;
}