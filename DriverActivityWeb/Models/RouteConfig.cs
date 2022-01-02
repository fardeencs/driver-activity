namespace DriverActivityWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RouteConfig
    {
        [Key]
        public long RouteConfigID { get; set; }
        [Required]
        public string RouteCode { get; set; }
        public string? RouteName { get; set; }
        public int? DeliveryCountSLA { get; set; }
        public decimal? ExtraDeliveryCharge { get; set; }
        public string? Description { get; set; }
        

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
