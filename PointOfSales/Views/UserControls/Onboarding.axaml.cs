using Avalonia.Interactivity;
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

    private void NavigateToBackOffice(object? sender, RoutedEventArgs e)
    {
        App.MainWindowInstance.NavigateTo(new BackOffice.BackOffice());
    }
}