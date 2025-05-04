using PointOfSales.Core.Data;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UnitOfWork(MyDbContext context, 
    IPermissionRepository permissionRepository,
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    IUserGroupRepository userGroupRepository,
    IGroupPermissionRepository groupPermissionRepository)
    : IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; } = permissionRepository;
    public IUserRepository UserRepository { get; } = userRepository;
    public IGroupRepository GroupRepository { get; } = groupRepository;
    public IUserGroupRepository UserGroupRepository { get; } = userGroupRepository;
    public IGroupPermissionRepository GroupPermissionRepository { get; } = groupPermissionRepository;

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}