namespace PointOfSales.Core.Entities.pos;

public class Shift : BaseEntity
{
    public int ShiftId { get; set; }
    public int BusinessDayId { get; set; }
    public int DeviceId { get; set; } // Renamed from MachineId to DeviceId

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public int? OpenedBy { get; set; }
    public int? ClosedBy { get; set; }

    public string? Status { get; set; }

    // Navigation
    public BusinessDay? BusinessDay { get; set; }
    public ICollection<UserShift>? UserShifts { get; set; }
}
