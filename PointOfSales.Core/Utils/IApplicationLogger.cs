namespace PointOfSales.Core.Utils;

public interface IApplicationLogger
{
    public void LogInfo(string message,params object[] args);
    public void LogWarning(string message, params object[] args);
    public void LogError(string message, params object[] args);
}