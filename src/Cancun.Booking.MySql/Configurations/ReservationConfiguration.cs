using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cancun.Booking.MySql.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<ReservationOrder>
    {
        public void Configure(EntityTypeBuilder<ReservationOrder> builder)
        {
            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.CustomerEmail)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.StayDays)
                .IsRequired();

            builder.Property(c => c.RoomId)
                .IsRequired();

        }
    }
}
