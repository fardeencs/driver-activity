using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriverActivityWeb.Models
{
    public class AppUser
    {
        [Key]
        public long UserId { get; set; }
        [ForeignKey("AppUserImage")]
        public string? FileID { get; set; }
        [Required]
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int? StaffId { get; set; }
        public int? QID { get; set; }
        [ForeignKey("VehicleType")]
        public int? VehicleID { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        
    }
}
