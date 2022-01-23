using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DriverActivityWeb.CustomAttributes
{
    public class AccessTokenActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var principal = context.HttpContext.User as ClaimsPrincipal;

            var accessTokenClaim = principal?.Claims
              .FirstOrDefault(c => c.Type == "access_token");

            if (accessTokenClaim is null || string.IsNullOrEmpty(accessTokenClaim.Value))
            {
                context.HttpContext.Response.Redirect("/account/login", permanent: true);

                return;
            }

            var sharedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456"));

            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                //ValidIssuer = "http://localhost:5000",
                //ValidAudiences = new[] { "http://localhost:5001" },
                IssuerSigningKeys = new[] { sharedKey }
            };

            var accessToken = accessTokenClaim.Value;

            var handler = new JwtSecurityTokenHandler();

            var user = (ClaimsPrincipal)null;

            try
            {
                user = handler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenValidationException exception)
            {
                throw new Exception($"Token failed validation: {exception.Message}");
            }

            base.OnActionExecuting(context);
        }
    }
}
