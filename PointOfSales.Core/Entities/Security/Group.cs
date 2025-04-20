
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Entities.Security
{
    public class Group : BaseEntity
    {
        [Key]
        public short GroupId { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
