using System;
using System.Threading.Tasks;
using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IEngines;
using PointOfSales.Engine.Utils;

namespace PointOfSales.Engines;

public class AuditLogEngine(IUnitOfWork unitOfWork):IAuditLogEngine
{
    public async Task AddLogAsync(short permissionId,string message, int userId)
    {
        var activityLog = new ActivityLog
        {
            PermissionId = permissionId,
            LocationId = Configurations.StoreId,
            DeviceId = Configurations.MachineId,
            Message = message,
            AccessedAt = DateTime.UtcNow,
            IsSuccess = true,
            UserId = userId
        };
        await unitOfWork.AuditLogRepository.WriteToLogAsync(activityLog);
    }
}