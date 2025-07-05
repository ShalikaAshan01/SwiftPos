using PointOfSales.Core.Entities.pos;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class ShiftRepository: GenericRepository<Shift>, IShiftRepository
{
    public ShiftRepository(MyDbContext context) : base(context)
    {
    }
}