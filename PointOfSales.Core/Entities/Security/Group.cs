
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Security
{
    [Table(nameof(Group), Schema = Schemas.SecuritySchema)]
    public class Group : BaseEntity
    {
        [Key]
        public short GroupId { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
