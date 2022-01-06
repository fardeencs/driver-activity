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
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    //    private List<User> _users = new List<User>
    //{
    //    new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    //};

        private readonly AppSettings _appSettings;
        private readonly IAppUserService appUserService;

        public UserService(IOptions<AppSettings> appSettings,
            IAppUserService appUserService)
        {
            _appSettings = appSettings.Value;
            this.appUserService = appUserService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = this.appUserService.GetUser(model.Username, model.Password);
            //var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        // helper methods

        private string generateJwtToken(UserVM user)
        {
            // generate token that is valid for 30 min
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
