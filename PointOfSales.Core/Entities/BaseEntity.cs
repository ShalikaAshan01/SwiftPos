namespace PointOfSales.Core.Entities
{
    public class BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? CompanyCode {get; set;}
        public int? LocationCode {get; set;}
    }
}
