namespace PointOfSales.Core.Utils;

public interface IEncryptionService
{
    public string EncryptPasswordAsync(string password, out string salt);
    public bool VerifyPassword(string inputPassword, string storedHashBase64, string saltBase64);
}