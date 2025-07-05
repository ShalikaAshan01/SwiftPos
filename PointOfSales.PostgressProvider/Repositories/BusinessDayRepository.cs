using PointOfSales.Core.Entities.pos;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class BusinessDayRepository: GenericRepository<BusinessDay>, IBusinessDayRepository
{
    public BusinessDayRepository(MyDbContext context) : base(context)
    {
    }
}