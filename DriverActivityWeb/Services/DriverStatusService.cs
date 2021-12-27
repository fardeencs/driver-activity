using AutoMapper;
using DriverActivityWeb.Contracts;
using DriverActivityWeb.Data;
using DriverActivityWeb.Models.DriverStatus;
using DriverActivityWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DriverActivityWeb.Services
{
    public class DriverStatusService : IDriverStatusService
    {
        private readonly DriverDbContext _driverDbContext;

        public DriverStatusService(DriverDbContext driverDbContext)
        {
            this._driverDbContext = driverDbContext;
        }

        public async Task<PaginatedList<ViewDriverDeliveryStatusVM>> GetDriverDeliveryStatus(SearchEntity message)
        {
            var query = from s in this._driverDbContext.ViewDriverDeliveryStatus
                        select s;

            var seletQuery = query.Select(s => new ViewDriverDeliveryStatusVM { 
              StaffId = Convert.ToString(s.StaffId),
              CreatedDate = s.CreatedDate,
              Delivered = s.Delivered,
              Drops = s.Drops,
              DutyStatus = s.DutyStatus,
              Failed = s.Failed,
              Name = s.Name,
              Total = s.Total,
            });
            message.PageSize = message.PageSize ?? 10;
            var result = await PaginatedList<ViewDriverDeliveryStatusVM>.CreateAsync(seletQuery.AsNoTracking(), message.PageNumber ?? 1, message.PageSize.Value);
            return result;

        }
    }
}
