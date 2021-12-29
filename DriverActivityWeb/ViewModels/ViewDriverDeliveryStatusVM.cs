namespace DriverActivityWeb.ViewModels
{
    public class ViewDriverDeliveryStatusVM : AppUserVM
    {
        //public int? StaffId { get; set; }
        //public string? Name { get; set; }
        public int? Total { get; set; }
        public int? Delivered { get; set; }
        public int? Failed { get; set; }
        public int? Drops { get; set; }
        public DateTime? ViewCreatedDate { get; set; }
        public string? DutyStatus { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
