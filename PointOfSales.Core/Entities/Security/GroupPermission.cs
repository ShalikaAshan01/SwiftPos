using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Security
{
    [Table(nameof(GroupPermission), Schema = Schemas.SecuritySchema)]
    public class GroupPermission : BaseEntity
    {
        [Key]
        public int GroupPermissionId { get; set; }
        public short GroupId { get; set; }
        public short PermissionId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsMfaRequired { get; set; } = false;
    }
}
