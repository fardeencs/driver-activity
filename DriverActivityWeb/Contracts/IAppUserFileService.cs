using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IAppUserFileService
    {
        Task<bool> ChangeProfileStatus(string fileID, bool status, long userId);
        Task<bool> DeleteProfileData(string fileID, long userId);
        Task<AppUserFileVM> SaveOrUpdateProfileData(AppUserFileVM message);
        Task<AppUserFileVM> SaveOrUpdateSignatureData(AppUserFileVM message);
        Task<string> ReadUserSignature(string fileId);
    }
}