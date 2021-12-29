using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DriverActivityWeb.Controllers
{
    public class DriverEodController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAppUserFileService _appUserFileService;
        private readonly IDriverCommonService driverCommonService;
        private readonly IDriverEODService driverEODService;

        public DriverEodController(ILogger<HomeController> logger,
            ApplicationDbContext dbContext,
            IAppUserFileService appUserFileService,
            IDriverCommonService driverCommonService,
            IDriverEODService driverEODService)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this._appUserFileService = appUserFileService;
            this.driverCommonService = driverCommonService;
            this.driverEODService = driverEODService;
        }

       

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> SearchDriverData(SearchEntity message)
        {
            var response = await this.driverCommonService.GetDriverDeliveryStatus(message);
            return Json(new ResponseEntity(response?.FirstOrDefault()));
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

    }
}