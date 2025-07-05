using PointOfSales.Core.Entities.Infrastructure;

namespace PointOfSales.Core.IRepositories;

public interface IDeviceRepository: IGenericRepository<Device>
{
    public Task<Device?> GetByUniqueCode(string uniqueCode);
}