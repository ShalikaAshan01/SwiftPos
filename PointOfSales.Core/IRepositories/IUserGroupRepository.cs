using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IRepositories;

public interface IUserGroupRepository : IGenericRepository<UserGroup>
{
    public Task<List<(short permissionId, bool isMfaRequred)>> GetUserGroupPermissionsByUserId(int userId);
}