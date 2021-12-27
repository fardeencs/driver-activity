using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IAppUserImageService
    {
        Task<bool> ChangeStatus(string fileID, bool status, long userId);
        Task<bool> DeleteData(string fileID, long userId);
        Task<AppUserImageVM> SaveOrUpdateData(AppUserImageVM message);
    }
}