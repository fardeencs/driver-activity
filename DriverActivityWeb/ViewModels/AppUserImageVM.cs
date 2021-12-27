namespace DriverActivityWeb.ViewModels
{
    public class AppUserImageVM : BaseEntity
    {
        public string FileID { get; set; }
        public byte[] ImgContent { get; set; }
        public IFormFile? File { get; set; }
    }
}
