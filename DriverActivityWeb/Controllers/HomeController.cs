using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DriverActivityWeb.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDriverCommonService driverCommonService;
        private readonly IApplicationService applicationService;

        public HomeController(ILogger<HomeController> logger,
            IDriverCommonService driverCommonService,
            IApplicationService applicationService)
        {
            _logger = logger;
            this.driverCommonService = driverCommonService;
            this.applicationService = applicationService;
        }

       

        public async Task<IActionResult> Index()
        {
            var vehicleType = await this.driverCommonService.GetVehicleType();
            string[] settingKeys = { Constant.PROFILE_UPLOAD_COUNT, Constant.PROFILE_UPLOAD_SIZE, Constant.PROFILE_UPLOAD_TYPE };
           var settings = await this.applicationService.GetSetting(settingKeys.ToList());
            DriverVM model = new DriverVM();
            model.VehicleTypes = vehicleType;
            model.SystemSettings = settings;
            return View(model);
        }

        
        public IActionResult DriverEOD()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}