using System.Security.Cryptography;
using PointOfSales.Core.Utils;

namespace PointOfSales.PostgressProvider.Utils;

public class EncryptionService : IEncryptionService
{
    public string EncryptPasswordAsync(string password, out string salt)
    {            
        var saltBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        salt = Convert.ToBase64String(saltBytes);

        // Create the hashed password
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }
}