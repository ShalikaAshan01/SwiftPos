using Avalonia.Controls;
using PointOfSales.ViewModels;

namespace PointOfSales.Views.UserControls.BackOffice.Logs;

public partial class ActivityLogViewer : UserControl
{
    public ActivityLogViewer()
    {
        InitializeComponent();
        DataContext = new ActivityLogViewerViewModel();
    }
}