using DriverActivityWeb.Models;

namespace DriverActivityWeb.Contracts
{
    public interface IApplicationService
    {
        Task<IEnumerable<SystemSetting>> GetSetting(List<string> bgKeys);
        Task<SystemSetting> GetSetting(string bgKey);
    }
}