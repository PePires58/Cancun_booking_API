using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Cancun.Booking.SqlServer.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ReservationConfiguration : IEntityTypeConfiguration<ReservationOrder>
    {
        public void Configure(EntityTypeBuilder<ReservationOrder> builder)
        {
            builder.ToTable("TB_RESERVATION_ORDER");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.StartDate)
                .HasColumnName("START_DATE");

            builder.Property(c => c.EndDate)
                .HasColumnName("END_DATE");

            builder.Property(c => c.CustomerEmail)
                .HasColumnName("CUST_EMAIL")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);

            builder.Property(c => c.Status)
                .HasColumnType("INT")
                .HasConversion(v => Convert.ToInt32(v), v => (ReservationOrderStatus)v)
                .IsRequired(true);

            builder.Property(c => c.RoomId)
                .HasColumnName("ROOM_ID")
                .HasColumnType("INT");
        }
    }
}
