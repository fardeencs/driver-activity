namespace DriverActivityWeb.ViewModels
{
    public class RouteConfigVM : BaseEntity
    {
        public long? RouteConfigID { get; set; }
        public string? RouteCode { get; set; }
        public string? RouteName { get; set; }
        public int? DeliveryCountSLA { get; set; }
        public decimal? ExtraDeliveryCharge { get; set; }
        public string? Description { get; set; }
    }
}
