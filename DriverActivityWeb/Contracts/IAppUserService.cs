using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IAppUserService
    {
        bool IsValidUser(string userName, string password);
        bool IsValidUser(string userName);
        UserVM? GetUser(long userId);
        UserVM? GetUser(string userName, string password);
        Task<List<AppUserVM>> GetData(SearchEntity message);
        Task<bool> ChangeStatus(long id, bool status, long userId);
        Task<bool> DeleteData(long id, long userId);
        Task<AppUserVM> SaveOrUpdateData(AppUserVM message);
    }
}