namespace DriverActivityWeb.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DriverEod
    {
        [Key]
        public long DriverEodID { get; set; }
        public int? StaffId { get; set; }
        [ForeignKey("AppUser")]
        public long? UserId { get; set; }
        public int? TotalDelivery { get; set; }
        public int? Delivered { get; set; }
        public int? FailedDelivery { get; set; }
        public int? Drops { get; set; }
        public int? AdditionalDelivery { get; set; }
        public int? Earned { get; set; }
        public string? Remarks { get; set; }
        [ForeignKey("AppUserSignature")]
        public string? FileID { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
