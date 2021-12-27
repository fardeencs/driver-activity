using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DriverActivityWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAppUserFileService _appUserFileService;
        private readonly IAppUserService appUserService;
        private readonly IDriverCommonService driverCommonService;
        private readonly IApplicationService applicationService;
        private readonly IDriverEODService driverEODService;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext dbContext,
            IAppUserFileService appUserImageService,
            IAppUserService appUserService,
            IDriverCommonService driverCommonService,
            IApplicationService applicationService,
            IDriverEODService driverEODService)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this._appUserFileService = appUserImageService;
            this.appUserService = appUserService;
            this.driverCommonService = driverCommonService;
            this.applicationService = applicationService;
            this.driverEODService = driverEODService;
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

        public async Task<JsonResult> GetDriverData(SearchEntity message)
        {
            var response = await this.appUserService.GetData(message);
            //var test = await this.driverCommonService.GetDriverDeliveryStatus(message);
            return Json(new ResponseEntity(response));
        }


        [HttpPost]
        public async Task<IActionResult> SaveProfileImage(long? userId)
        {

            var file = Request.Form.Files[0];
            if (file == null)
                throw new BadHttpRequestException("File not found");

           
            byte[]? imageBytes = file.ConvertToByteArray();
            AppUserFileVM appUserImage = new AppUserFileVM
            {
                SessionUserId = 1,
                IsActive = false,
                ImgContent = imageBytes
            };

            var response = await this._appUserFileService.SaveOrUpdateProfileData(appUserImage);
            return Json(new ResponseEntity(response));
        }

        [HttpPost]
        public async Task<IActionResult> SaveSignatureImage(long? userId)
        {

            var file = Request.Form.Files[0];
            if (file == null)
                throw new BadHttpRequestException("File not found");


            byte[]? imageBytes = file.ConvertToByteArray();
            AppUserFileVM appUserImage = new AppUserFileVM
            {
                SessionUserId = 1,
                IsActive = false,
                ImgContent = imageBytes
            };

            var response = await this._appUserFileService.SaveOrUpdateSignatureData(appUserImage);
            return Json(new ResponseEntity(response));
        }


        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateUserData([FromBody] AppUserVM request)
        {
            if (request == null)
                throw new BadHttpRequestException("Request params not found");

            if (AppUtility.IsEmpty(request.FileID))
                throw new CustomException("Kindly upload the profile picture.");

            request.SessionUserId = 1;
            var response = await this.appUserService.SaveOrUpdateData(request);
            return Json(new ResponseEntity(response));
        }


        public IActionResult DriverEOD()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateDriverEOD([FromBody] DriverEodVM request)
        {
            if (request == null)
                throw new BadHttpRequestException("Request params not found");

            request.SessionUserId = 1;
            AppUserFileVM appUserImage = new AppUserFileVM
            {
                SessionUserId = 1,
                IsActive = false,
                FileString = request.Signature
            };

            var fileResponse = await this._appUserFileService.SaveOrUpdateSignatureData(appUserImage);
            request.FileID = fileResponse.FileID;
            var response = await this.driverEODService.SaveOrUpdateData(request);
            return Json(new ResponseEntity(response));
        }

        public async Task<IActionResult> GetUserSignature(string id)
        {
            if (AppUtility.IsEmpty(id))
                throw new KeyNotFoundException("File ID not found");

            var response = await this._appUserFileService.ReadUserSignature(id);
            return Json(new ResponseEntity(response));
        }



        public IActionResult Privacy()
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