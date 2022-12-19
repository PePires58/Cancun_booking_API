using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Cancun.Booking.SqlServer.Context
{
    [ExcludeFromCodeCoverage]
    public class CancunDbContext : DbContext
    {
        public CancunDbContext(DbContextOptions<CancunDbContext> options) : base(options) { }

        public DbSet<ReservationOrder> ReservationOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
