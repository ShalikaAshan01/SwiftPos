namespace PointOfSales.Core.Entities.pos;
public class BusinessDay : BaseEntity
{
    public int BusinessDayId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Status { get; set; }

    // Navigation
    public ICollection<Shift>? Shifts { get; set; }
}
