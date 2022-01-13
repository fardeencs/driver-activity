using DriverActivityWeb.Models;
using DriverActivityWeb.Services;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IDriverCommonService
    {
        UserVM? GetSessionUserName();
        Task<IEnumerable<VehicleType>> GetVehicleType(int id = 0);
        Task<PaginatedList<ViewDriverDeliveryStatusVM>> GetDriverDeliveryStatus(SearchEntity message);
        Task<decimal?> GetRouteDeliveryCharges(string routeCode);
    }
}