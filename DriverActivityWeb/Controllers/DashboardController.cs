using DriverActivityWeb.Contracts;
using DriverActivityWeb.Helper;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DriverActivityWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppUserService appUserService;
        private readonly IDriverCommonService driverCommonService;
        private readonly IApplicationService applicationService;

        public DashboardController(ILogger<HomeController> logger,
            IAppUserService appUserService,
            IDriverCommonService driverCommonService,
            IApplicationService applicationService)
        {
            _logger = logger;
            this.appUserService = appUserService;
            this.driverCommonService = driverCommonService;
            this.applicationService = applicationService;
        }

        private async Task<int> GetPageSize()
        {
           var settings = await this.applicationService.GetSetting(Constant.PAGE_SIZE);
           return Convert.ToInt32(settings.Value);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetDriverDeliveryStatus(SearchEntity message)
        {
            message.PageSize = await this.GetPageSize();
            var response = await this.driverCommonService.GetDriverDeliveryStatus(message);
            return Json(new ResponseEntity(response));
        }

    }
}
