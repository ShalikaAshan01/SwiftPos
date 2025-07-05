using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine;

public class LocationEngine(IUnitOfWork unitOfWork): ILocationEngine
{
    public Task<Location?> GetLocationById(short id)
    {
        return unitOfWork.LocationRepository.GetByIdAsync(id);
    }
}