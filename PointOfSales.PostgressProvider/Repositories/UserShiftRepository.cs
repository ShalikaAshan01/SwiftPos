using PointOfSales.Core.Entities.pos;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class UserShiftRepository : GenericRepository<UserShift>, IUserShiftRepository
{
    public UserShiftRepository(MyDbContext context) : base(context)
    {
    }
}