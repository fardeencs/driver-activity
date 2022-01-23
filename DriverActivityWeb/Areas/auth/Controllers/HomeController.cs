using DriverActivityWeb.Contracts;
using DriverActivityWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverActivityWeb.Areas.Login.Controllers
{
    [Area("auth")]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private readonly IAppUserService appUserService;

        public HomeController(IUserService userService,
            IAppUserService appUserService)
        {
            _userService = userService;
            this.appUserService = appUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = _userService.Authenticate(request);
            return Ok(new ResponseEntity(response));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                throw new CustomException("Request params not found");

            request.SessionUserId = 1;
            var response = await this.appUserService.SaveOrUpdateData(request);
            return Json(new ResponseEntity(response));
        }


        [HttpGet, Authorize]
        public IActionResult GetMe()
        {
            var result = _userService.GetSessionUserName();
            return Ok(new ResponseEntity(result));
        }



       
         
        
           /* //var userName = User?.Identity?.Name;
            //var userName2 = User.FindFirstValue(ClaimTypes.Name);
            //var role = User.FindFirstValue(ClaimTypes.Role);
            //return Ok(new { userName, userName2, role });
         //return View(response);
        return RedirectToAction("Index", "Home", new { area = "login" });*/
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
            //return View(request);*/
         
        
    }
}
