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

            builder.Property(c => c.StartDate)
                .HasColumnName("START_DATE");

            builder.Property(c => c.EndDate)
                .HasColumnName("END_DATE");

            builder.Property(c => c.CustomerEmail)
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
        }
    }
}
