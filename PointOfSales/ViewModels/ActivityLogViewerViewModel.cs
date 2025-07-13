using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IEngines;

namespace PointOfSales.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class ActivityLogViewerViewModel : ObservableObjectBase
{
    public ObservableCollection<ActivityLog> Logs { get; } = new();
    public ICommand ReloadCommand { get; }
    [ObservableProperty] private bool _isLoading = false;

    public ActivityLogViewerViewModel()
    {
        ReloadCommand = new AsyncRelayCommand(LoadLogsAsync);
        _ = DelayedInitialLoadAsync();
    }
    private async Task DelayedInitialLoadAsync()
    {
        await Task.Delay(100); 
        await LoadLogsAsync();
    }

    private async Task LoadLogsAsync()
    {
        var engine = GetEngine<IAuditLogEngine>();

        (List<ActivityLog> res,int pages) = await engine.SearchLogAsync(new Dictionary<string, object>() { }, 1, 1000);
        foreach (var log in res)
        {
            Logs.Add(log);
        }
    }
}
