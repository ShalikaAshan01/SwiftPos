using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Entities.Infrastructure;

namespace PointOfSales.Core.Entities.Security;

[Table(nameof(ActivityLog), Schema = Schemas.SecuritySchema)]
public class ActivityLog
{
    [Key]
    public Guid ActivityLogId { get; set; }
    public short PermissionId { get; set; }
    public int? UserId { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime AccessedAt { get; set; }
    public int? OverrideUserId { get; set; }
    public string? Message { get; set; }
    public short LocationId { get; set; }
    public short DeviceId { get; set; }
    
    public virtual Permission? Permission { get; set; }
    public virtual User? User { get; set; }
    public virtual Location? Location { get; set; }
    public virtual Device? Device { get; set; }
}