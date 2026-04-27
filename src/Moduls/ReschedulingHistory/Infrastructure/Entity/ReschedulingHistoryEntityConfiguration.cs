using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Infrastructure.Entity;

public sealed class ReschedulingHistoryEntityConfiguration : IEntityTypeConfiguration<ReschedulingHistoryEntity>
{
    public void Configure(EntityTypeBuilder<ReschedulingHistoryEntity> builder)
    {
        builder.ToTable("flight_rescheduling_history");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(x => x.PreviousFlightId)
            .HasColumnName("previous_flight_id")
            .IsRequired();

        builder.Property(x => x.NewFlightId)
            .HasColumnName("new_flight_id")
            .IsRequired();

        builder.Property(x => x.ChangedAt)
            .HasColumnName("changed_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(x => x.Booking)
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .HasConstraintName("fk_rescheduling_booking")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PreviousFlight)
            .WithMany()
            .HasForeignKey(x => x.PreviousFlightId)
            .HasConstraintName("fk_rescheduling_previous_flight")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.NewFlight)
            .WithMany()
            .HasForeignKey(x => x.NewFlightId)
            .HasConstraintName("fk_rescheduling_new_flight")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BookingId);
        builder.HasIndex(x => x.PreviousFlightId);
        builder.HasIndex(x => x.NewFlightId);
    }
}