using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IEngines;

public interface IAuditLogEngine
{
    public Task AddLogAsync(short permissionId,string message, int userId);
    
    public Task<(List<ActivityLog> results, int totalPages)> SearchLogAsync(Dictionary<string, object> filters, int page, int pageSize); 
}