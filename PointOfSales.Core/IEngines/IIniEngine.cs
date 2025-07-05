namespace PointOfSales.Core.IEngines;

public interface IIniEngine
{
    public Task InitAsync();
    public Task UpdateAsync(string section, Dictionary<string, object> settings);
    public T Read<T>(string section, string key);
    public Task UpdateAsync(string key, object value, string section="default");
    
}