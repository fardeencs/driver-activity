using DriverActivityWeb.Models.DriverStatus;
using DriverActivityWeb.Services;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IDriverStatusService
    {
        Task<PaginatedList<ViewDriverDeliveryStatusVM>> GetDriverDeliveryStatus(SearchEntity message);
    }
}