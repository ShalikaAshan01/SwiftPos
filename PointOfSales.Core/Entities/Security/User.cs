using PointOfSales.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointOfSales.Core.Entities.Security
{

    [Table(nameof(User), Schema = Schemas.SecuritySchema)]
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? PasswordExpiryDate { get; set; }
        public bool ShouldChangePassword { get; set; }
    }
}
