namespace PointOfSales.Core.Utils;

public interface IEncryptionService
{
    public string EncryptPasswordAsync(string password, out string salt);
}