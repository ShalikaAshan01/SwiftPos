namespace PointOfSales.Core.Entities.pos;

public class UserShift : BaseEntity
{
    public int UserShiftId { get; set; }
    public int ShiftId { get; set; }
    public int UserId { get; set; }

    public DateTime SignOnTime { get; set; }
    public DateTime? SignOffTime { get; set; }

    public int? TempFromUserShiftId { get; set; }

    // Navigation
    public Shift? Shift { get; set; }
    public UserShift? TempFromUserShift { get; set; }
    public ICollection<UserShift>? TemporarySessions { get; set; } // inverse relationship
}
