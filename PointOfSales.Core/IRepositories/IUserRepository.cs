using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User?> GetByUsernameAsync(string username);
}