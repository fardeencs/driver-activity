namespace DriverActivityWeb.ViewModels
{
    public class AppUserVM : BaseEntity
    {
        public long? UserId { get; set; }
        public string? FileID { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? QID { get; set; }
        public string? StaffId { get; set; }
        public string? VehicleID { get; set; }
        public string VehicleName { get; set; }

        public BaseEntity Created { get; set; }
        public BaseEntity Updated { get; set; }
        public byte[] ImgContent { get; set; }
    }
}
