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

    [ObservableProperty] private bool _isLoading = false;
    [ObservableProperty] private int _currentPage = 1;
    [ObservableProperty] private int _totalPages = 1;
    [ObservableProperty] private int _pageSize = 20;

    public ICommand ReloadCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PrevPageCommand { get; }
    public ICommand FirstPageCommand { get; }
    public ICommand LastPageCommand { get; }

    public ActivityLogViewerViewModel()
    {
        ReloadCommand = new AsyncRelayCommand(LoadLogsAsync);
        NextPageCommand = new AsyncRelayCommand(NextPageAsync, () => CurrentPage < TotalPages);
        PrevPageCommand = new AsyncRelayCommand(PrevPageAsync, () => CurrentPage > 1);
        FirstPageCommand = new AsyncRelayCommand(FirstPageAsync, () => CurrentPage > 1);
        LastPageCommand = new AsyncRelayCommand(LastPageAsync, () => CurrentPage < TotalPages);

        _ = DelayedInitialLoadAsync();
    }

    private async Task DelayedInitialLoadAsync()
    {
        await Task.Delay(100);
        await LoadLogsAsync();
    }

    private async Task LoadLogsAsync()
    {
        IsLoading = true;
        try
        {
            var engine = GetEngine<IAuditLogEngine>();

            var parameters = new Dictionary<string, object>() { };
            (List<ActivityLog> res, int pages) = await engine.SearchLogAsync(parameters, CurrentPage, PageSize);

            Logs.Clear();
            foreach (var log in res)
            {
                Logs.Add(log);
            }

            TotalPages = pages;

            // Update CanExecute for paging commands
            ((AsyncRelayCommand)NextPageCommand).NotifyCanExecuteChanged();
            ((AsyncRelayCommand)PrevPageCommand).NotifyCanExecuteChanged();
            ((AsyncRelayCommand)FirstPageCommand).NotifyCanExecuteChanged();
            ((AsyncRelayCommand)LastPageCommand).NotifyCanExecuteChanged();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task NextPageAsync()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadLogsAsync();
        }
    }

    private async Task PrevPageAsync()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadLogsAsync();
        }
    }

    private async Task FirstPageAsync()
    {
        if (CurrentPage != 1)
        {
            CurrentPage = 1;
            await LoadLogsAsync();
        }
    }

    private async Task LastPageAsync()
    {
        if (CurrentPage != TotalPages)
        {
            CurrentPage = TotalPages;
            await LoadLogsAsync();
        }
    }
}
