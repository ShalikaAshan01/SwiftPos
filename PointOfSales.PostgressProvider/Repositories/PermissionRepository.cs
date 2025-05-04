using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;
using PointOfSales.Core.Utils;

namespace PointOfSales.PostgressProvider.Repositories;

public class PermissionRepository(MyDbContext context, IApplicationLogger logger) : GenericRepository<Permission>(context), IPermissionRepository
{
    public override async Task<List<Permission>> SaveListAsync(List<Permission> entities)
    {
        var all = await base.GetAllAsync();
        var permissionsToAdd = entities.Except(all).ToList();
        if (permissionsToAdd.Count == 0)
            return [];
        logger.LogInfo("Inserting permission codes: {0}", string.Join(",",permissionsToAdd.Select(s=> s.PermissionCode).ToList()));
        await base.SaveListAsync(permissionsToAdd.ToList());
        return entities;
    }
}