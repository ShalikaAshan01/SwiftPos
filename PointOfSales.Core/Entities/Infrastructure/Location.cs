using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Infrastructure;

[Table(nameof(Location), Schema = Schemas.Infrastructure)]
public class Location : BaseEntity
{
    public new short LocationId { get; set; }
    public string LocationName { get; set; }
    public string LocationCode { get; set; }
    public byte? CompanyId { get; set; }
}