using PointOfSales.Core.Entities.Infrastructure;

namespace PointOfSales.Core.IEngines;

public interface IDeviceEngine
{
    public Task<Device> RegisterDeviceAsync(String uniqueCode, short locationId);
    public Task<Device?> GetDeviceByUniqueCode(String uniqueCode);
}