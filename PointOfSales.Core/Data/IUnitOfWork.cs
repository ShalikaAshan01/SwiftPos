using PointOfSales.Core.Entities.pos;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.Core.Data;

public interface IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; }
    public IUserRepository UserRepository { get; }
    public IGroupRepository GroupRepository { get; }
    public IUserGroupRepository UserGroupRepository { get; }
    public IGroupPermissionRepository GroupPermissionRepository { get; }
    public IAuditLogRepository AuditLogRepository { get; }
    public IDeviceRepository DeviceRepository { get; }
    public ILocationRepository LocationRepository { get; }
    public IBusinessDayRepository BusinessDayRepository { get; }
    public IUserShiftRepository UserShiftRepository { get; }
    public IShiftRepository ShiftRepository { get; }
    public ICompanyRepository CompanyRepository { get; }
    Task SaveChangesAsync();
}