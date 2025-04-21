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
        [Required]
        public required string PermissionName { get; set; }
    }
}
