using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointOfSales.Core.Constants;

public partial class LogViewerViewModel : ObservableObject
{
    private const string LogFilePath = "logs/app.log";

    [ObservableProperty] private string filterText = string.Empty;

    [ObservableProperty] private bool isLoading = false;

    public ObservableCollection<LogEntry> AllLogs { get; } = new();
    public ObservableCollection<LogEntry> FilteredLogs { get; } = new();

    public ICommand ReloadCommand { get; }

    public LogViewerViewModel()
    {
        ReloadCommand = new AsyncRelayCommand(LoadLogsAsync);
        _ = LoadLogsAsync(); // Initial load
    }

    partial void OnFilterTextChanged(string value)
    {
        ApplyFilter();
    }

    private async Task LoadLogsAsync()
    {
        IsLoading = true;

        try
        {
            AllLogs.Clear();

            string logsDir = Path.Combine(LocalConfigurations.LocalFolderPath, "Logs");

            if (Directory.Exists(logsDir))
            {
                var logFiles = Directory.GetFiles(logsDir, "*.log", SearchOption.TopDirectoryOnly).Reverse();
                foreach (var file in logFiles)
                {
                    var lines = await File.ReadAllLinesAsync(file);
                    foreach (var line in lines)
                    {
                        var entry = LogEntry.Parse(line);
                        AllLogs.Add(entry);
                    }
                }
            }
            else
            {
                AllLogs.Add(new LogEntry
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Level = "WARN",
                    Message = $"Log directory not found: {logsDir}"
                });
            }

            ApplyFilter();
        }
        catch (Exception ex)
        {
            AllLogs.Clear();
            AllLogs.Add(new LogEntry
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Level = "ERROR",
                Message = $"Failed to load logs: {ex.Message}"
            });
            ApplyFilter();
        }

        IsLoading = false;
    }

    private void ApplyFilter()
    {
        FilteredLogs.Clear();
        if(FilterText.Length < 3 && FilterText.Length != 0) return;

        var filtered = string.IsNullOrWhiteSpace(FilterText)
            ? AllLogs
            : AllLogs.Where(l =>
                (l.Message?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Level?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Timestamp?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false));

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

        return new LogEntry
        {
            Timestamp = timestamp,
            Level = level,
            Message = message
        };
    }
}