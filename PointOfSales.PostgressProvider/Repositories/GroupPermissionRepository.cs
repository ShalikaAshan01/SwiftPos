using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class GroupPermissionRepository(MyDbContext context)
    : GenericRepository<GroupPermission>(context), IGroupPermissionRepository;