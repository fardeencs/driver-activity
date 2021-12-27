using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IAppUserService
    {
        Task<List<AppUserVM>> GetData(SearchEntity message);
        Task<bool> ChangeStatus(long id, bool status, long userId);
        Task<bool> DeleteData(long id, long userId);
        Task<AppUserVM> SaveOrUpdateData(AppUserVM message);
    }
}