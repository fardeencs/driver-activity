namespace DriverActivityWeb.ViewModels
{
    public class AppUserFileVM : BaseEntity
    {
        public string FileID { get; set; }
        public byte[] ImgContent { get; set; }
        public string? FileString { get; set; }
        public IFormFile? File { get; set; }
        public bool? IsTemp { get; set; } = false;
    }
}
