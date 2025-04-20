using System.Text;
using System.Text.Json;
using PointOfSales.Core.Constants;
using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine;

public class IniEngine: IIniEngine
{
    private readonly string _filePath = Path.Combine(LocalConfigurations.LocalFolderPath, LocalConfigurations.IniFile);
    private readonly Dictionary<string, Dictionary<string, string>> _data = new();

    public async Task InitAsync()
    {
        if (!File.Exists(_filePath))
        {
            await using var writer = File.CreateText(_filePath);
            await writer.WriteLineAsync("; Auto-generated settings file");
            return;
        }

        string? currentSection = null;

        foreach (var line in await File.ReadAllLinesAsync(_filePath))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";"))
                continue;

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
            {
                currentSection = trimmed[1..^1];
                if (!_data.ContainsKey(currentSection))
                    _data[currentSection] = new Dictionary<string, string>();
            }
            else if (currentSection != null && trimmed.Contains('='))
            {
                var parts = trimmed.Split('=', 2);
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                _data[currentSection][key] = value;
            }
        }
    }

    public async Task UpdateAsync(string section, Dictionary<string, object> settings)
    {
        if (!_data.ContainsKey(section))
            _data[section] = new Dictionary<string, string>();

        foreach (var kvp in settings)
        {
            var json = JsonSerializer.Serialize(kvp.Value);
            _data[section][kvp.Key] = json;
        }

        var builder = new StringBuilder();
        builder.AppendLine("; Updated settings");

        foreach (var sec in _data)
        {
            builder.AppendLine($"[{sec.Key}]");
            foreach (var kvp in sec.Value)
            {
                builder.AppendLine($"{kvp.Key} = {kvp.Value}");
            }
            builder.AppendLine();
        }

        await File.WriteAllTextAsync(_filePath, builder.ToString());
    }

    public T Read<T>(string section, string key)
    {
        if (_data.TryGetValue(section, out var sectionDict))
        {
            if (sectionDict.TryGetValue(key, out var raw))
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(raw)!;
                }
                catch
                {
                    return (T)Convert.ChangeType(raw, typeof(T))!;
                }
            }
        }

        throw new KeyNotFoundException($"Key '{key}' not found in section '{section}'");
    }
}