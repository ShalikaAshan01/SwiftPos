using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Entities.Security
{
    public class Permission : BaseEntity
    {
        [Key]
        public short PermissionId { get; set; }
        [Required]
        public required string PermissionName { get; set; }
    }
}
