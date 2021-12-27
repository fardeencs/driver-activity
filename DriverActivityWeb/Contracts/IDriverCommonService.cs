﻿using DriverActivityWeb.Models;
using DriverActivityWeb.Services;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IDriverCommonService
    {
        Task<IEnumerable<VehicleType>> GetVehicleType(int id = 0);
        Task<PaginatedList<ViewDriverDeliveryStatusVM>> GetDriverDeliveryStatus(SearchEntity message);
    }
}