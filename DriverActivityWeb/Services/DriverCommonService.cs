using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DriverActivityWeb.Services
{
    public class DriverCommonService : IDriverCommonService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDriverStatusService driverStatusService;
        private readonly IAppUserService appUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DriverCommonService(ApplicationDbContext dbContext,
            IDriverStatusService driverStatusService,
            IAppUserService appUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this.driverStatusService = driverStatusService;
            this.appUserService = appUserService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<VehicleType>> GetVehicleType(int id = 0)
        {
            if (id > 0)
                return await this._dbContext.VehicleType.Where(x => x.VehicleID == id).ToListAsync();
            else
                return await this._dbContext.VehicleType.ToListAsync();

        }

        public async Task<decimal?> GetRouteDeliveryCharges(string routeCode)
        {
            if(AppUtility.IsEmpty(routeCode)) return 0;

            var routeConfig = await this._dbContext.RouteConfig.Where(x => x.RouteCode == routeCode).FirstOrDefaultAsync();
            return routeConfig?.ExtraDeliveryCharge ?? 0;
        }


        public UserVM? GetSessionUserName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = (UserVM)_httpContextAccessor.HttpContext.Items["User"];
                //result = _httpContextAccessor.HttpContext.User.FindFirstValue (ClaimTypes.Name);
                return user;
            }
            return null;
        }

        public async Task<PaginatedList<ViewDriverDeliveryStatusVM>> GetDriverDeliveryStatus(SearchEntity message)
        {
            if(message.IsSearch.HasValue && message.IsSearch.Value && AppUtility.IsNotEmpty(message.QID))
            {
                var driverOtherData = await this.appUserService.GetData(new SearchEntity { QID = message.QID });
                if(driverOtherData != null && driverOtherData.Count > 0)
                {
                    message.StaffId = Convert.ToInt32(driverOtherData.FirstOrDefault().StaffId);
                    var deliverStatus = await this.driverStatusService.GetDriverDeliveryStatus(message);
                    UpdateDriverStatusData(deliverStatus, driverOtherData);
                    return deliverStatus;
                }
                return null;
            }
            else
            {
                var deliverStatus = await this.driverStatusService.GetDriverDeliveryStatus(message);
                if (null != deliverStatus && deliverStatus.Count > 0)
                {
                    var staffIds = deliverStatus.Where(w => AppUtility.IsNotEmpty(w.StaffId) && Convert.ToInt32(w.StaffId) > 0)
                                        .Select(x => Convert.ToInt32(x.StaffId)).ToList();
                    var driverOtherData = await this.appUserService.GetData(new SearchEntity { StaffIds = staffIds });
                    UpdateDriverStatusData(deliverStatus, driverOtherData);
                }
                return deliverStatus;
            }
        }

        private static void UpdateDriverStatusData(PaginatedList<ViewDriverDeliveryStatusVM> deliverStatus, List<AppUserVM> driverOtherData)
        {
            foreach (var item in driverOtherData)
            {
                var exsitItem = deliverStatus.Where(x => AppUtility.IsNotEmpty(x.StaffId)
                            && Convert.ToInt32(x.StaffId) == Convert.ToInt32(item.StaffId))
                            .FirstOrDefault();
                if (null != exsitItem)
                {
                    exsitItem.UserId = item.UserId;
                    exsitItem.CreatedDate = item.CreatedDate;
                    //exsitItem.ViewCreatedDate = item.ViewCreatedDate;
                    exsitItem.StaffId = item.StaffId;
                    exsitItem.QID = item.QID;
                    exsitItem.VehicleID = item.VehicleID;
                    exsitItem.VehicleName = item.VehicleName;
                    exsitItem.UserId = item.UserId;
                    byte[] imgContent = item.ImgContent;
                    if (imgContent != null && imgContent.Count() > 0)
                    {
                        string base64String = Convert.ToBase64String(imgContent, 0, imgContent.Length);
                        exsitItem.ProfilePicture = "data:image/png;base64," + base64String;
                    }
                }
            }
        }
    }
}
