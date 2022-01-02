namespace DriverActivityWeb.Controllers
{
    using DriverActivityWeb.Contracts;
    using DriverActivityWeb.Helper;
    using DriverActivityWeb.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    

    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                throw new BadHttpRequestException("Username or password is incorrect");

            return Ok(response);
        }
    }
}
