using Microsoft.EntityFrameworkCore;

namespace DriverActivityWeb.Data
{
    using DriverActivityWeb.Models;
   

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserImage> AppUserImage { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }
        public DbSet<DriverEod> DriverEod { get; set; }
        public DbSet<AppUserSignature> AppUserSignature { get; set; }
        public DbSet<RouteConfig> RouteConfig { get; set; }

    }

}
