namespace DriverActivityWeb.Models.DriverStatus
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;


    public class ViewDriverDeliveryStatus
    {
        [Key]
        public int? StaffId { get; set; }
        public string? Name { get; set; }
        public int? Total { get; set; }
        public int? Delivered { get; set; }
        public int? Failed { get; set; }
        public int? Drops { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DutyStatus { get; set; }
        public string? Route { get; set; }
    }
}
