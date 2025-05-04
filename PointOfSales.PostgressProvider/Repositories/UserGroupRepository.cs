using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UserGroupRepository(MyDbContext context) : GenericRepository<UserGroup>(context), IUserGroupRepository;