namespace DriverActivityWeb.ViewModels
{
    public class SearchEntity
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public List<int> StaffIds { get; set; }
        public int? StaffId { get; set; }
    }
}
