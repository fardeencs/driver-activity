using Microsoft.AspNetCore.Mvc;
using DriverActivityWeb.Contracts;
using DriverActivityWeb.Helper;
using DriverActivityWeb.ViewModels;
using DriverActivityWeb.CustomAttributes;

namespace DriverActivityWeb.Controllers
{
    [Authorize]
    [Route("api/driver")]
    [ApiController]
    public class DriverApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppUserFileService _appUserFileService;
        private readonly IAppUserService appUserService;
        private readonly IDriverCommonService driverCommonService;
        private readonly IApplicationService applicationService;
        private readonly IDriverEODService driverEODService;

        public DriverApiController(ILogger<HomeController> logger,
            IAppUserFileService appUserImageService,
            IAppUserService appUserService,
            IDriverCommonService driverCommonService,
            IApplicationService applicationService,
            IDriverEODService driverEODService)
        {
            _logger = logger;
            this._appUserFileService = appUserImageService;
            this.appUserService = appUserService;
            this.driverCommonService = driverCommonService;
            this.applicationService = applicationService;
            this.driverEODService = driverEODService;
        }


        #region driver-form

        [HttpGet]
        [Route("GetDriverData")]
        public async Task<ResponseEntity> GetDriverData([FromQuery] SearchEntity message)
        {
            var response = await this.appUserService.GetData(message);
            //var test = await this.driverCommonService.GetDriverDeliveryStatus(message);
            return new ResponseEntity(response);
        }


        [HttpPost]
        [Route("SaveProfileImage")]
        public async Task<ResponseEntity> SaveProfileImage(long? userId)
        {

            var file = Request.Form.Files[0];
            if (file == null)
                throw new BadHttpRequestException("File not found");


            byte[]? imageBytes = file.ConvertToByteArray();
            AppUserFileVM appUserImage = new AppUserFileVM
            {
                SessionUserId = 1,
                IsActive = false,
                IsTemp  = true,
                ImgContent = imageBytes
            };

            var response = await this._appUserFileService.SaveOrUpdateProfileData(appUserImage);
            return new ResponseEntity(response);
        }

        [HttpPost]
        [Route("SaveSignatureImage")]
        public async Task<ResponseEntity> SaveSignatureImage(long? userId)
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
            return new ResponseEntity(response);
        }


        [HttpPost]
        [Route("SaveOrUpdateUserData")]
        public async Task<ResponseEntity> SaveOrUpdateUserData([FromBody] AppUserVM request)
        {
            if (request == null)
                throw new BadHttpRequestException("Request params not found");

            if (AppUtility.IsEmpty(request.FileID))
                throw new CustomException("Kindly upload the profile picture.");

            request.SessionUserId = 1;
            var response = await this.appUserService.SaveOrUpdateData(request);
            return new ResponseEntity(response);
        }


        #endregion

        #region driver-eod

        [HttpGet]
        [Route("SearchDriverData")]
        public async Task<ResponseEntity> SearchDriverData([FromQuery] SearchEntity message)
        {
            var response = await this.driverCommonService.GetDriverDeliveryStatus(message);
            var result = response?.FirstOrDefault();
            if (result != null)
            {
                result.ExtraDeliveryCharge = await this.driverCommonService.GetRouteDeliveryCharges(result.Route);
            }
            return new ResponseEntity(result);
        }

        [HttpPost]
        [Route("SaveOrUpdateDriverEOD")]
        public async Task<ResponseEntity> SaveOrUpdateDriverEOD([FromBody] DriverEodVM request)
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
            return new ResponseEntity(response);
        }

        [HttpGet]
        [Route("GetUserSignature")]
        public async Task<ResponseEntity> GetUserSignature(string id)
        {
            if (AppUtility.IsEmpty(id))
                throw new KeyNotFoundException("File ID not found");

            var response = await this._appUserFileService.ReadUserSignature(id);
            return new ResponseEntity(response);
        }
        #endregion

    }
}
