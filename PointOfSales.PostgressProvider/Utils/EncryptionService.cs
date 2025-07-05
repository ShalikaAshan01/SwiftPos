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
    
    public bool VerifyPassword(string inputPassword, string storedHashBase64, string saltBase64)
    {
        var saltBytes = Convert.FromBase64String(saltBase64);
        using var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, saltBytes, 10000, HashAlgorithmName.SHA256);
        var inputHash = pbkdf2.GetBytes(32);
        var storedHash = Convert.FromBase64String(storedHashBase64);

        return CryptographicOperations.FixedTimeEquals(inputHash, storedHash);
    }

}