namespace PointOfSales.Core.IEngines;

public interface IAuditLogEngine
{
    public Task AddLogAsync(short permissionId,string message, int userId);
}