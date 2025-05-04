using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Utils;

namespace PointOfSales.PostgressProvider.Repositories;

public class UserRepository(MyDbContext context, IEncryptionService encryptionService) : GenericRepository<User>(context), IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username)
    {
        return _dbSet.FirstOrDefaultAsync(u => string.Equals(u.UserName, username.ToLowerInvariant()));
    }

    public override Task<User> SaveAsync(User entity)
    {
        entity.UserName = entity.UserName.ToLowerInvariant();
        entity.Password = encryptionService.EncryptPasswordAsync(entity.Password, out var salt);
        entity.Salt = salt;
        return base.SaveAsync(entity);
    }
}