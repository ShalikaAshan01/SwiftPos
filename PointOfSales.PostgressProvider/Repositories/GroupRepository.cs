using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class GroupRepository(MyDbContext context) : GenericRepository<Group>(context), IGroupRepository
{
    public Task<Group?> GetByNameAsync(string name)
    {
        return _dbSet
            .Include(g => g.GroupPermissions)
            .ThenInclude(ug => ug.Permission)
            .FirstOrDefaultAsync(g => string.Equals(g.Name.ToLower(), name.Trim().ToLowerInvariant()));
    }
}