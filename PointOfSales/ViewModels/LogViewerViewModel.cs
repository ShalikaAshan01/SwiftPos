using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointOfSales.Core.Constants;

namespace PointOfSales.ViewModels;

public partial class LogViewerViewModel : ObservableObject
{
    [ObservableProperty] private string _filterText = string.Empty;
    [ObservableProperty] private bool _isLoading = false;
    [ObservableProperty] private string _selectedDate = string.Empty;

    public ObservableCollection<LogEntry> FilteredLogs { get; } = new();
    public ObservableCollection<string> AvailableDates { get; } = new();

    private readonly Dictionary<string, List<LogEntry>> _logsByDate = new();

    public ICommand ReloadCommand { get; }

    public LogViewerViewModel()
    {
        ReloadCommand = new AsyncRelayCommand(LoadLogsAsync);
        _ = LoadLogsAsync();
    }

    partial void OnFilterTextChanged(string value)
    {
        ApplyFilter();
    }

    partial void OnSelectedDateChanged(string value)
    {
        ApplyFilter();
    }

    private async Task LoadLogsAsync()
    {
        IsLoading = true;
        _logsByDate.Clear();
        AvailableDates.Clear();
        FilteredLogs.Clear();

        string logsDir = Path.Combine(LocalConfigurations.LocalFolderPath, "Logs");

        if (Directory.Exists(logsDir))
        {
            var logFiles = Directory.GetFiles(logsDir, "*.log").OrderByDescending(x => x);
            foreach (var file in logFiles)
            {
                var lines = await File.ReadAllLinesAsync(file);
                foreach (var line in lines)
                {
                    var entry = LogEntry.Parse(line);
                    if (!DateTime.TryParse(entry.Timestamp, out var dt)) continue;
                    string key = dt.ToShortDateString();
                    if (!_logsByDate.ContainsKey(key))
                        _logsByDate[key] = new List<LogEntry>();

                    _logsByDate[key].Add(entry);
                }
            }

            foreach (var dateStr in _logsByDate.Keys
                         .Select(s => DateTime.Parse(s, CultureInfo.CurrentCulture))
                         .OrderByDescending(d => d))
            {
                AvailableDates.Add(dateStr.ToShortDateString());
            }

            // Default today if exists
            var today = DateTime.Today.ToShortDateString();
            if (AvailableDates.Contains(today))
            {
                SelectedDate = today;
            }
            else
            {
                SelectedDate = AvailableDates.FirstOrDefault() ?? string.Empty;
            }

            ApplyFilter();
        }

        IsLoading = false;
    }

    private void ApplyFilter()
    {
        FilteredLogs.Clear();

        if (string.IsNullOrWhiteSpace(SelectedDate) || !_logsByDate.ContainsKey(SelectedDate))
            return;

        var logs = _logsByDate[SelectedDate];
        logs.Reverse();
        var filtered = string.IsNullOrWhiteSpace(FilterText)
            ? logs
            : logs.Where(l =>
                (l.Message.Contains(FilterText, StringComparison.OrdinalIgnoreCase)) ||
                (l.Level.Contains(FilterText, StringComparison.OrdinalIgnoreCase)) ||
                (l.Timestamp.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));

        foreach (var entry in filtered)
        {
            FilteredLogs.Add(entry);
        }
    }
}

public class LogEntry
{
    public string Timestamp { get; set; } = "";
    public string Level { get; set; } = "";
    public string Message { get; set; } = "";

    public IBrush LevelColor =>
        Level.ToUpper() switch
        {
            "ERROR" => Brushes.Red,
            "WARN" => Brushes.Orange,
            "INFO" => Brushes.Green,
            _ => Brushes.Gray
        };

    public static LogEntry Parse(string line)
    {
        // Example: "[2025-07-07 12:34:56] [INFO] Application started."
        var parts = line.Split("] ");
        if (parts.Length < 2) return new LogEntry { Message = line };

        var timestamp = parts[0].Trim('[', ']');
        var level = parts[1].Trim('[', ']');
        var message = string.Join("] ", parts.Skip(2));
        if (DateTime.TryParse(timestamp, out var dt))
        {
            timestamp = dt.ToString(CultureInfo.CurrentCulture);
        }

        return new LogEntry
        {
            Timestamp = timestamp,
            Level = level,
            Message = message
        };
    }
}