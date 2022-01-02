using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Contracts
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}
