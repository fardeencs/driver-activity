using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IDriverEODService
    {
        Task<DriverEodVM> SaveOrUpdateData(DriverEodVM message);
    }
}