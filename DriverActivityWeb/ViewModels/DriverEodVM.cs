namespace DriverActivityWeb.ViewModels
{
    public class DriverEodVM : BaseEntity
    {
        public long? DriverEodID { get; set; }
        public string? FileID { get; set; }
        public int? TotalDelivery { get; set; }
        public int? Delivered { get; set; }
        public int? FailedDelivery { get; set; }
        public int? Drops { get; set; }
        public int? AdditionalDelivery { get; set; }
        public string? Remarks { get; set; }
        public string? Signature { get; set; }
    }
}
