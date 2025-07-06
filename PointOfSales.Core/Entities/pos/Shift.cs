namespace PointOfSales.Core.Entities.pos;

public class Shift : BaseEntity
{
    public int ShiftId { get; set; }
    public int BusinessDayId { get; set; }
    public int DeviceId { get; set; } 
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public int? OpenedBy { get; set; }
    public int? ClosedBy { get; set; }

    public double OpeningBalance { get; set; }
    public double ClosingBalance { get; set; }

    // Navigation
    public BusinessDay? BusinessDay { get; set; }
    public ICollection<UserShift>? UserShifts { get; set; }
}
