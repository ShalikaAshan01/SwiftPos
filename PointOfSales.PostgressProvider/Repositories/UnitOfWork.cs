using PointOfSales.Core.Data;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UnitOfWork(MyDbContext context, 
    IPermissionRepository permissionRepository,
    IUserRepository userRepository)
    : IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; } = permissionRepository;
    public IUserRepository UserRepository { get; } = userRepository;

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}