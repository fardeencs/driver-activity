using DriverActivityWeb.Contracts;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost("authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        //public IActionResult Authenticate(AuthenticateRequest request)
        {
            var isValid = ModelState.IsValid;
            var response = _userService.Authenticate(request);

            if (response == null)
                throw new UnauthorizedAccessException("Username or password is incorrect");

            return Ok(new ResponseEntity(response));
            //return View(response);

            //if (ModelState.IsValid)
            //{
            //    //var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);
            //    var result = _userService.Authenticate(request);
            //    if (null != result)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }

            //    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            //}
            //return View(request);
        }
    }
}
