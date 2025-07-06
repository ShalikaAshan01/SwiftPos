using PointOfSales.Core.Data;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UnitOfWork(
    MyDbContext context,
    IPermissionRepository permissionRepository,
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    IUserGroupRepository userGroupRepository,
    IGroupPermissionRepository groupPermissionRepository,
    IAuditLogRepository auditLogRepository,
    IDeviceRepository deviceRepository,
    ILocationRepository locationRepository,
    IBusinessDayRepository businessDayRepository,
    IShiftRepository shiftRepository,
    IUserShiftRepository userShiftRepository,
    ICompanyRepository companyRepository)
    : IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; } = permissionRepository;
    public IUserRepository UserRepository { get; } = userRepository;
    public IGroupRepository GroupRepository { get; } = groupRepository;
    public IUserGroupRepository UserGroupRepository { get; } = userGroupRepository;
    public IGroupPermissionRepository GroupPermissionRepository { get; } = groupPermissionRepository;
    public IAuditLogRepository AuditLogRepository { get; } = auditLogRepository;
    public IDeviceRepository DeviceRepository { get; } = deviceRepository;
    public ILocationRepository LocationRepository { get; } = locationRepository;
    public IBusinessDayRepository BusinessDayRepository { get; } = businessDayRepository;
    public IShiftRepository ShiftRepository { get; } = shiftRepository;
    public IUserShiftRepository UserShiftRepository { get; } = userShiftRepository;
    public ICompanyRepository CompanyRepository { get; } = companyRepository;

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}