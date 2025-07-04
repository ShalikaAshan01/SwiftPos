using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.Commands;

public interface ILoginCommand
{
    public Task<string> CheckUserNameAsync(string username);
    public Task<User> LoginAsync(string username, string password);
}