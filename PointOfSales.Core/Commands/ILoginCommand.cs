using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.Commands;

public interface ILoginCommand
{
    public Task<User> LoginAsync(string username, string password);
}