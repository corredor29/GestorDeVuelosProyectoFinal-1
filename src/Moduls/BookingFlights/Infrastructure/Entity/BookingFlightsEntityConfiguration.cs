using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;

public sealed class BookingFlightsEntityConfiguration : IEntityTypeConfiguration<BookingFlightsEntity>
{
    public void Configure(EntityTypeBuilder<BookingFlightsEntity> builder)
    {
        builder.ToTable("booking_flights", table =>
        {
            table.HasCheckConstraint("chk_partial_amount", "partial_amount >= 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(x => x.FlightId)
            .HasColumnName("flight_id")
            .IsRequired();

        builder.Property(x => x.PartialAmount)
            .HasColumnName("partial_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasIndex(x => new { x.BookingId, x.FlightId })
            .IsUnique();

        builder.HasIndex(x => x.BookingId);
        builder.HasIndex(x => x.FlightId);

        builder.HasOne(x => x.Booking)
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .HasConstraintName("fk_booking_flights_booking")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Flight)
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .HasConstraintName("fk_booking_flights_flight")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
