using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Security
{
    [Table(nameof(Permission),Schema = Schemas.SecuritySchema)]
    public class Permission : BaseEntity
    {
        [Key]
        public short PermissionId { get; set; }
        [Required] public required string PermissionName { get; set; }
        [Required]
        [MaxLength(5)]
        public required string PermissionCode { get; set; }
        public virtual ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if(obj is not Permission objPermission) return false;
            return PermissionCode == objPermission.PermissionCode;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return PermissionCode?.GetHashCode() ?? 0;
        }

    }
}
