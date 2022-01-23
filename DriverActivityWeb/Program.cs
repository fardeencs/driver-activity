using DriverActivityWeb.CustomAttributes;
using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Middleware;
using FluentValidation.AspNetCore;
using JwtGenerator.Extensions;
using JwtGenerator.Types;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var key = Guid.NewGuid().ToString();
var tokenOptions = new TokenOptions("ISS", "AUD", key);

builder.Services.AddJwtAuthenticationWithProtectedCookie(tokenOptions, "app");

builder.Services.AddControllersWithViews();

/*builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});*/

//var SecretKey = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");

/*builder.Services.AddAuthentication(options =>
        {
            // these must be set other ASP.NET Core will throw exception that no
            // default authentication scheme or default challenge scheme is set.
            options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
        })
    //.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.LoginPath = "/login/Account/Signin";
               options.LogoutPath = "/login/Account/Signout";
               options.Cookie.Expiration = TimeSpan.FromMinutes(5);
               options.Cookie.HttpOnly = true;
               options.Cookie.SecurePolicy = CookieSecurePolicy.None;
               options.Cookie.SameSite = SameSiteMode.Lax;
           })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = true,
                    //Usually this is your application base URL - JRozario
                    //ValidIssuer = "http://localhost:45092/",
                    ValidateAudience = true,
                    //Here we are creating and using JWT within the same application. In this case base URL is fine - JRozario
                    //If the JWT is created using a web service then this could be the consumer URL - JRozario
                    //ValidAudience = "http://localhost:45092/",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });*/



builder.Services.AddMvc(o =>
{
    o.Filters.Add(new ValidationFilterAttribute());
    //o.Filters.Add(new AuthorizeFilter());
    //o.Filters.Add(new RequireHttpsAttribute())
}).AddFluentValidation(v =>
{
    //v.RegisterValidatorsFromAssemblyContaining(AppDomain.CurrentDomain.GetAssemblies());
    v.DisableDataAnnotationsValidation = true;
    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});


builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContextPool<DriverDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DriverDBConnection")));

ServiceResolver.Resolver(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

//Addd User session - JRozario
//app.UseSession();

//Add JWToken to all incoming HTTP Request Header - JRozario
/*app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});
//Add JWToken Authentication service - JRozario
app.UseAuthentication();*/

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// custom jwt auth middleware
//app.UseMiddleware<JwtMiddleware>();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "default",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );

//    //endpoints.MapAreaControllerRoute(
//    //  name: "default",
//    //  pattern: "{controller=Home}/{action=Index}/{id?}"
//    //);
//});

/*app.MapAreaControllerRoute(name: "login_route",
                                     areaName: "login",
                                     pattern: "{area:exists}/{controller}/{action}/{id?}");*/

/*app.MapAreaControllerRoute("login_route", "login",
        "login/{controller}/{action}/{id?}");
*/

app.MapControllerRoute(
     name: "auth_route",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");





app.Run();
