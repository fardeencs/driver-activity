namespace DriverActivityWeb.Services
{
    using DriverActivityWeb.Contracts;
    using DriverActivityWeb.Helper;
    using DriverActivityWeb.ViewModels;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    
    public class UserService : IUserService
    {
       
        private readonly AppSettings _appSettings;
        private readonly IAppUserService appUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IOptions<AppSettings> appSettings,
            IAppUserService appUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            this.appUserService = appUserService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = this.appUserService.GetUser(model.Username, model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string GenerateToken(UserVM user)
        {
           /* List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key0 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            var creds = new SigningCredentials(key0, SecurityAlgorithms.HmacSha512Signature);

            // generate token that is valid for 30 min
            var token0 = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: creds);*/
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("username", user.Username) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserVM? GetSessionUserName()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = (UserVM)_httpContextAccessor.HttpContext.Items["User"];
                //result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                return user;
            }
            return null;
        }

    }
}
