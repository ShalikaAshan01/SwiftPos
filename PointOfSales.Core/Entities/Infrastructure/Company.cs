using System.ComponentModel.DataAnnotations.Schema;
using PointOfSales.Core.Constants;

namespace PointOfSales.Core.Entities.Infrastructure;

[Table(nameof(Company), Schema = Schemas.Infrastructure)]
public class Company : BaseEntity
{
    public byte CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string? CompanyCode { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyWebsite { get; set; }
}