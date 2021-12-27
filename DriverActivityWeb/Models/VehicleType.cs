namespace DriverActivityWeb.Models
{
    using System.ComponentModel.DataAnnotations;
    

    public class VehicleType
    {
        [Key]
        public int VehicleID { get; set; }
        [Required]
        public string Vehicle { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
