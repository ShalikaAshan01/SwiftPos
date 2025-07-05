using PointOfSales.Core.Entities.Infrastructure;

namespace PointOfSales.Core.IEngines;

public interface ILocationEngine
{
    public Task<Location?> GetLocationById(short id);
}