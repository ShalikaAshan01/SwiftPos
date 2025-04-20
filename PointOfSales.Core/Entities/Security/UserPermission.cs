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
        public DateTime EffectiveDate { get; set; }
        public bool IsMfaRequired { get; set; } = false;
    }
}
