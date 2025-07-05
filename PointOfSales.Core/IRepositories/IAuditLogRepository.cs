using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IRepositories;

public interface IAuditLogRepository
{
    public Task WriteToLogAsync(ActivityLog log);
}