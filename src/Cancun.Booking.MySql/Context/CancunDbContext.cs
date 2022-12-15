using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.MySql.Context
{
    public class CancunDbContext : DbContext
    {
        public CancunDbContext(DbContextOptions<CancunDbContext> options) : base(options) { }

        public DbSet<ReservationOrder> ReservationOrders { get; set; }
    }
}
