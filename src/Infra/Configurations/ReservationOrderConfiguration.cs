using Domain.Entities;
using Domain.Enuns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations
{
    internal class ReservationOrderConfiguration : IEntityTypeConfiguration<ReservationOrder>
    {
        public void Configure(EntityTypeBuilder<ReservationOrder> builder)
        {
            builder.ToTable("TB_RESERVATION_ORDER");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.StartDate)
                .HasColumnName("START_DATE");

            builder.Property(c => c.EndDate)
                .HasColumnName("END_DATE");

            builder.Property(c => c.CustomerEmail)
                .HasColumnName("CUST_EMAIL")
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(c => c.Status)
                .HasConversion(v => Convert.ToInt32(v), v => (ReservationStatus)v)
                .IsRequired(true);

            builder.Property(c => c.RoomId)
                .HasColumnName("ROOM_ID");

            builder.Ignore(c => c.StayDays);
        }
    }
}
