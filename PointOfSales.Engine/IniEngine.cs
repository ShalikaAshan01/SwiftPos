using System.Text.Json;
using PointOfSales.Core.Constants;
using PointOfSales.Core.IEngines;

public class IniEngine : IIniEngine
{
    private readonly string _filePath = Path.Combine(LocalConfigurations.LocalFolderPath, LocalConfigurations.IniFile);
    private readonly Dictionary<string, Dictionary<string, string>> _data = new();
    private readonly List<string> _rawLines = new(); // preserves original lines

    public async Task InitAsync()
    {
        if (!File.Exists(_filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            await File.WriteAllTextAsync(_filePath, "; Auto-generated settings file\n");
        }

        _rawLines.Clear();
        _rawLines.AddRange(await File.ReadAllLinesAsync(_filePath));

        string? currentSection = null;

        foreach (var line in _rawLines)
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
        foreach (var kvp in settings)
        {
            var json = JsonSerializer.Serialize(kvp.Value);
            await UpdateAsync(kvp.Key, json, section);
        }
    }

    public async Task UpdateAsync(string key, object value, string section = "default")
    {
        await InitAsync(); // Ensure _rawLines and _data are loaded
        var serializedValue = JsonSerializer.Serialize(value);

        bool sectionExists = false;
        bool keyUpdated = false;
        var output = new List<string>();
        string? currentSection = null;

        foreach (var line in _rawLines)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
            {
                if (currentSection == section && !keyUpdated)
                {
                    // append key=value before moving to next section
                    output.Add($"{key} = {serializedValue}");
                    keyUpdated = true;
                }

                currentSection = trimmed[1..^1];
                sectionExists |= currentSection == section;
                output.Add(line);
                continue;
            }

            if (currentSection == section && trimmed.StartsWith(key + " ="))
            {
                output.Add($"{key} = {serializedValue}");
                keyUpdated = true;
            }
            else
            {
                output.Add(line);
            }
        }

        if (!sectionExists)
        {
            output.Add($"[{section}]");
            output.Add($"{key} = {serializedValue}");
        }
        else if (!keyUpdated)
        {
            output.Add($"{key} = {serializedValue}");
        }

        await File.WriteAllLinesAsync(_filePath, output);
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
