namespace DriverActivityWeb.ViewModels
{
    public abstract class BaseEntity
    {
        public long? SessionUserId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Name { get; set; }
    }
}
