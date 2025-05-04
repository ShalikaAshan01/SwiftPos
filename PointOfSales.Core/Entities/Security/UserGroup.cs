using PointOfSales.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointOfSales.Core.Entities.Security
{
    [Table(nameof(UserGroup), Schema = Schemas.SecuritySchema)]
    public class UserGroup : BaseEntity
    {
        [Key]
        public int UserGroupId { get; set; }
        [Required]
        public short GroupId { set; get; }
        [Required]
        public int UserId { set; get; }

        public virtual User User { set; get; } = null!;
        public virtual Group Group { set; get; } = null!;
    }
}
