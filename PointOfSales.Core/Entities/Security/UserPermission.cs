using PointOfSales.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointOfSales.Core.Entities.Security
{
    [Table(nameof(UserPermission), Schema = Schemas.SecuritySchema)]
    public class UserPermission : BaseEntity
    {
        [Key]
        public int UserPermissionId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public short PermissionId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsMfaRequired { get; set; } = false;
        
        public virtual User User { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}
