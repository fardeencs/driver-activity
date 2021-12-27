using Microsoft.EntityFrameworkCore;

namespace DriverActivityWeb.Data
{
    using DriverActivityWeb.Models.DriverStatus;

    public class DriverDbContext : DbContext
    {
        public DriverDbContext(DbContextOptions<DriverDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ViewDriverDeliveryStatus> ViewDriverDeliveryStatus { get; set; }
    }

}
