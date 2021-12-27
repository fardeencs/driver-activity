using DriverActivityWeb.Models;

namespace DriverActivityWeb.ViewModels
{
    public class DriverVM : AppUserVM
    {
   
        public IEnumerable<VehicleType> VehicleTypes { get; set; }
        public IEnumerable<SystemSetting> SystemSettings { get; set; }
    }
}
