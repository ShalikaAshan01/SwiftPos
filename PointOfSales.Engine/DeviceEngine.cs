using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine;

public class DeviceEngine(IUnitOfWork unitOfWork) : IDeviceEngine
{
    public async Task<Device> RegisterDeviceAsync(string uniqueCode)
    {
        return await unitOfWork.DeviceRepository.SaveAsync(new Device
        {
            DeviceCode = string.Empty,
            MachineCode = uniqueCode,
            IsActive = false
        });
    }

    public Task<Device?> GetDeviceByUniqueCode(string uniqueCode)
    {
        return unitOfWork.DeviceRepository.GetByUniqueCode(uniqueCode);
    }
}