namespace DriverActivityWeb.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SystemSetting
    {
        [Key]
        public int SystemSettingID { get; set; }
        [Required]
        public string BgKey { get; set; }
        [Required]
        public string Value { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
