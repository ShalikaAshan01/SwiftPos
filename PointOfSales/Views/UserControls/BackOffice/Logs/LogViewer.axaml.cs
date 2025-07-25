using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Threading;
using PointOfSales.ViewModels;

namespace PointOfSales.Views.UserControls.BackOffice.Logs
{
    public partial class LogViewer : UserControl
    {
        public LogViewer()
        {
            InitializeComponent();
            DataContext = new LogViewerViewModel();

            DataContextChanged += LogViewer_DataContextChanged;

            if (DataContext is LogViewerViewModel vm)
            {
                SubscribeToLogs(vm);
            }
        }

        private void LogViewer_DataContextChanged(object? sender, dynamic e)
        {
            if (e.NewValue is LogViewerViewModel vm)
            {
                SubscribeToLogs(vm);
            }
        }

        private void SubscribeToLogs(LogViewerViewModel vm)
        {
            // Unsubscribe first to avoid duplicate subscriptions if DataContext changes multiple times
            vm.FilteredLogs.CollectionChanged -= FilteredLogs_CollectionChanged;
            vm.FilteredLogs.CollectionChanged += FilteredLogs_CollectionChanged;
        }

        private void FilteredLogs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Scroll to bottom on UI thread when logs update
            Dispatcher.UIThread.Post(() =>
            {
                LogScrollViewer?.ScrollToEnd();
            });
        }
    }
}