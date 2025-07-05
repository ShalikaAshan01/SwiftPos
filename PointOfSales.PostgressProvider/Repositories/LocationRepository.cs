using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class LocationRepository: GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(MyDbContext context) : base(context)
    {
    }
}