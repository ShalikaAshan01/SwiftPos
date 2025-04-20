using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Entities.Security
{
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
