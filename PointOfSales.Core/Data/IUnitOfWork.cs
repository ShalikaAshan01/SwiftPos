using PointOfSales.Core.IRepositories;

namespace PointOfSales.Core.Data;

public interface IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; }
    public IUserRepository UserRepository { get; }
    public IGroupRepository GroupRepository { get; }
    public IUserGroupRepository UserGroupRepository { get; }
    public IGroupPermissionRepository GroupPermissionRepository { get; }
    Task SaveChangesAsync();
}