using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IRepositories;

public interface IGroupRepository : IGenericRepository<Group>
{
    public Task<Group?> GetByNameAsync(string name);
}