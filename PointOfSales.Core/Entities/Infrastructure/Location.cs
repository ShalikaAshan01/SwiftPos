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
    public string? LocationAddress1 { get; set; }
    public string? LocationAddress2 { get; set; }
    public string? LocationCity { get; set; }
    public string? LocationState { get; set; }
    public string? LocationZipCode { get; set; }
    public string? LocationCountry { get; set; }
    public string? LocationPhone { get; set; }
    public string? LocationEmail { get; set; }

    public string GetFullAddress()
    {
        var parts = new List<string>();

        AddIfNotNullOrEmpty(LocationAddress1);
        AddIfNotNullOrEmpty(LocationAddress2);
        AddIfNotNullOrEmpty(LocationCity);
        AddIfNotNullOrEmpty(LocationState);
        AddIfNotNullOrEmpty(LocationZipCode);
        AddIfNotNullOrEmpty(LocationCountry);

        return string.Join(", ", parts);

        void AddIfNotNullOrEmpty(string? part)
        {
            if (!string.IsNullOrWhiteSpace(part))
                parts.Add(part.Trim());
        }
    }
}