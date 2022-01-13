namespace DriverActivityWeb.CustomAttributes
{
    using DriverActivityWeb.Helper;
    using DriverActivityWeb.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserVM)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                var templateName = context.ActionDescriptor.AttributeRouteInfo.Template;
                if (AppUtility.IsNotEmpty(templateName) && templateName.StartsWith("api/"))
                {
                    context.Result = new JsonResult(ApiResponse<string>.Fail("Unauthorized")) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    context.HttpContext.Response.Redirect("/login/home");
                }
                //throw new UnauthorizedAccessException();
            }
        }
    }
}
