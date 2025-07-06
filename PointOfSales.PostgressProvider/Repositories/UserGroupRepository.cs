using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UserGroupRepository(MyDbContext context) : GenericRepository<UserGroup>(context), IUserGroupRepository
{
    public async Task<List<(short permissionId, bool isMfaRequred)>> GetUserGroupPermissionsByUserId(int userId)
    {
        var userGroups = await _dbSet.Where(u => u.UserId == userId)
            .Include(u => u.Group)
            .ThenInclude(g => g.GroupPermissions)
            .ThenInclude(g => g.Permission)
            .ToListAsync();

        return userGroups
            .SelectMany(u => u.Group.GroupPermissions)
            .Select(gp => (gp.PermissionId, gp.IsMfaRequired))
            .ToList();
    }
}