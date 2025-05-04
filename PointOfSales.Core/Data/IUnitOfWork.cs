using PointOfSales.Core.IRepositories;

namespace PointOfSales.Core.Data;

public interface IUnitOfWork
{
    public IPermissionRepository PermissionRepository { get; }
    public IUserRepository UserRepository { get; }
    Task SaveChangesAsync();
}