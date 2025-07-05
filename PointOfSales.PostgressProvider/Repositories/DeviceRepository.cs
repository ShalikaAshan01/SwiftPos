using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class DeviceRepository: GenericRepository<Device>, IDeviceRepository
{
    public DeviceRepository(MyDbContext context) : base(context)
    {
    }

    public Task<Device?> GetByUniqueCode(string uniqueCode)
    {
        return _dbSet.FirstOrDefaultAsync(u => string.Equals(u.MachineCode.ToLower(), uniqueCode.ToLowerInvariant()));
    }
}