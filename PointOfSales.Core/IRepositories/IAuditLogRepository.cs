using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IRepositories;

public interface IAuditLogRepository
{
    public Task WriteToLogAsync(ActivityLog log);
    Task<(List<ActivityLog> result, int totalPages)> SearchAsync(Dictionary<string, dynamic> parameters, int pageNo = 1, int pageSize = 10);
}