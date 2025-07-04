using PointOfSales.Core.Commands;
using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Security;

namespace PointOfSales.PostgressProvider.Commands;

public class LoginCommand(IUnitOfWork unitOfWork): ILoginCommand
{
    public async Task<string> CheckUserNameAsync(string username)
    {
        var user = await unitOfWork.UserRepository.GetByUsernameAsync(username);
        return "Inactive User";
    }

    public Task<User> LoginAsync(string username, string password)
    {
        throw new NotImplementedException();
    }
}