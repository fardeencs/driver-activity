using DriverActivityWeb.Data;
using DriverActivityWeb.Helper;
using DriverActivityWeb.Middleware;
//using DriverActivityWeb.ModelValidator;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(o =>
{
    o.Filters.Add(new ValidationFilterAttribute());
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

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

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

app.MapControllerRoute(
     name: "default",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
