using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IUserService
    {
        UserVM? GetSessionUserName();
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}
