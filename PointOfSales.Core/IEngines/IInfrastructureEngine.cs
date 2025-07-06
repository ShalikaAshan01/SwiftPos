using PointOfSales.Core.Entities.Infrastructure;

namespace PointOfSales.Core.IEngines;

public interface IInfrastructureEngine
{
    public Task<Location?> GetLocationById(short id);
    public Task<Device> RegisterDeviceAsync(String uniqueCode, short locationId);
    public Task<Device?> GetDeviceByUniqueCode(String uniqueCode);
    public Task<Company?> GetCompanyById(byte id);
}