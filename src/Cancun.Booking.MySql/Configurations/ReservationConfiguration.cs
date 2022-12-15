using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Cancun.Booking.MySql.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ReservationConfiguration : IEntityTypeConfiguration<ReservationOrder>
    {
        public void Configure(EntityTypeBuilder<ReservationOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .HasColumnType("VARCHAR(36)")
                .HasConversion(v => v.ToString(), v => Guid.Parse(v));

            builder.Property(c => c.StartDate)
                .HasColumnName("START_DATE");

            builder.Property(c => c.EndDate)
                .HasColumnName("END_DATE");

            builder.Property(c => c.CustomerEmail)
                .HasColumnType("VARCHAR(255)");

            builder.Property(c => c.StayDays)
                .IsRequired();

            builder.Property(c => c.RoomId)
                .IsRequired();
        }
    }
}
