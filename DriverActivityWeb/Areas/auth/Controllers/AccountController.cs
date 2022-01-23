using DriverActivityWeb.Contracts;
using DriverActivityWeb.ViewModels;
using JwtGenerator.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DriverActivityWeb.Areas.Login.Controllers
{
    [Area("auth")]
    public class AccountController : Controller
    {

        private readonly IJwtTokenGenerator tokenGenerator;
        private readonly IAppUserService appUserService;

        public AccountController(IJwtTokenGenerator tokenGenerator,
            IAppUserService appUserService)
        {
            this.tokenGenerator = tokenGenerator;
            this.appUserService = appUserService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateRequest request, string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            var returnTo = "/Account/Login";

            // Replace this with your custom authentication logic which will
            // securely return the authenticated user's details including
            // any role specific info
            var userInfo = this.appUserService.GetUser(request.Username, request.Password);
            var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                   request.Username,
                   AddMyClaims(userInfo));
            await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                accessTokenResult.AuthProperties);
            returnTo = returnUrl;
            /*if (userCredentials.Username == "user1" && userCredentials.Password == "aman14")
            {
                var userInfo = new UserInfo
                {
                    FirstName = "UserFName",
                    LastName = "UserLName",
                    HasAdminRights = true
                };

                var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                    userCredentials.Username,
                    AddMyClaims(userInfo));
                await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                    accessTokenResult.AuthProperties);
                returnTo = returnUrl;
            }*/

            return RedirectToLocal(returnTo);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private static IEnumerable<Claim> AddMyClaims(UserVM authenticatedUser)
        {
            var myClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticatedUser.Name),
                new Claim(ClaimTypes.GivenName, authenticatedUser.Username),
                new Claim(ClaimTypes.PrimarySid, authenticatedUser.Id.ToString()),
                //new Claim("HasAdminRights", authenticatedUser.HasAdminRights ? "Y" : "N")
            };

            return myClaims;
        }

        /// <summary>
        /// Sign in
        /// </summary>
        /// <returns></returns>
        /*[HttpPost]
        public async Task<IActionResult> Signin()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "Jeffcky"),
                new Claim(JwtRegisteredClaimNames.Email, "2752154844@qq.com"),
                new Claim(JwtRegisteredClaimNames.Sub, "D21D099B-B49B-4604-A247-71B0518A0B1C"),
                new Claim("access_token", GenerateAccessToken()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authticationProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authticationProperties);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        string GenerateAccessToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456"));

            var token = new JwtSecurityToken(
                //issuer: "http://localhost:5000",
                //audience: "http://localhost:5001",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }*/

    }
}
