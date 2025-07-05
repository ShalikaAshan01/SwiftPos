using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class AuditLogRepository(MyDbContext context) : IAuditLogRepository
{
    public async Task WriteToLogAsync(ActivityLog log)
    {
        await context.Set<ActivityLog>().AddAsync(log);
    }
}