using DriverActivityWeb.Contracts;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DriverActivityWeb.Areas.Login.Controllers
{
    [Area("login")]
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                throw new UnauthorizedAccessException("Username or password is incorrect");

            return Ok(new ResponseEntity(response));
        }
    }
}
