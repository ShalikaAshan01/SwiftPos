using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine;

public class InfrastructureEngine(IUnitOfWork unitOfWork) : IInfrastructureEngine
{
    public async Task<Device> RegisterDeviceAsync(string uniqueCode, short locationId)
    {
        return await unitOfWork.DeviceRepository.SaveAsync(new Device
        {
            DeviceCode = string.Empty,
            MachineCode = uniqueCode,
            IsActive = false,
            LocationId = locationId,
        });
    }

    public Task<Device?> GetDeviceByUniqueCode(string uniqueCode)
    {
        return unitOfWork.DeviceRepository.GetByUniqueCode(uniqueCode);
    }

    public Task<Company?> GetCompanyById(byte id)
    {
        return unitOfWork.CompanyRepository.GetByIdAsync(id);
    }

    public Task<Location?> GetLocationById(short id)
    {
        return unitOfWork.LocationRepository.GetByIdAsync(id);
    }
}