using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Infrastructure;

[Table(nameof(Device), Schema = Schemas.Infrastructure)]
public class Device : BaseEntity
{
    public short DeviceId { get; set; }
    public string DeviceCode { get; set; }
    public string MachineCode { get; set; }
    public DateTime LastActiveTime { get; set; } 
}