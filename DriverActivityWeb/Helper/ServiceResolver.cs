using DriverActivityWeb.Contracts;
using DriverActivityWeb.Services;
using Newtonsoft.Json.Serialization;

namespace DriverActivityWeb.Helper
{
    public static class ServiceResolver
    {
        public static void Resolver(WebApplicationBuilder builder)
        {
            
            /*builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });*/

            builder.Services.AddScoped<IAppUserFileService, AppUserFileService>();
            builder.Services.AddScoped<IAppUserService, AppUserService>();
            builder.Services.AddScoped<IDriverCommonService, DriverCommonService>();
            builder.Services.AddScoped<IDriverStatusService, DriverStatusService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IDriverEODService, DriverEODService>();
            builder.Services.AddScoped<IUserService, UserService>();
            
        }
    }
}
