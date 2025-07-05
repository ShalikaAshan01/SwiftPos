using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Core.Constants
{
    public class PermissionCodes
    {
        public const string CreateUser = "U0001";
        public const string LoginToSystem = "AU001";

        public static Dictionary<string, Permission> Permissions { get; private set; } = new();

        public static void SetPermissions(List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                Permissions.Add(permission.PermissionCode, permission);
            }
        }

        public static Dictionary<string, string> GetPermissions() => new()
        {
            { "Create User", CreateUser },
            { "Login to System", LoginToSystem },
        };

        public static short GetPermissionId(string permissionCode)
        {
            return Permissions.GetValueOrDefault(permissionCode)?.PermissionId ?? 0;
        }
    }
}