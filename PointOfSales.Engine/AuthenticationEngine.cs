using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.Exceptions;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;

namespace PointOfSales.Engine;

public class AuthenticationEngine(IUnitOfWork unitOfWork, IEncryptionService encryptionService) : IAuthenticationEngine
{
    private async Task<User> GetUserAsync(string username)
    {
        var user = await unitOfWork.UserRepository.GetByUsernameAsync(username);
        Utils.Common.Logger.LogInfo("Login Clicked: Username: {0}, Password:<PASSWORD>", username);

        if (user == null || user.IsDeleted)
        {
            throw new SwiftException(Common.Resources.ApplicationErrors.UserNotFound, username);
        }

        if (!user.IsActive)
        {
            throw new SwiftException(Common.Resources.ApplicationErrors.UserNotFound);
        }

        Utils.Common.Logger.LogInfo("User({0}) found with the given username: {1}", user.UserId, username);
        return user;
    }

    public async Task CheckUserNameAsync(string username)
    {
        await GetUserAsync(username);
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await GetUserAsync(username);
        var passwordResult = encryptionService.VerifyPassword(password, user.Password, user.Salt);
        if (!passwordResult)
        {
            throw new SwiftException(Common.Resources.ApplicationErrors.InvalidPassword);
        }

        return user;
    }
}