using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.IEngines;

public interface IAuthenticationEngine
{
    public Task CheckUserNameAsync(string username);
    public Task<User> AuthenticateUserAsync(string username, string password);
    public Task<Dictionary<short, bool>> GetPermissionsAsync(int userId);
}