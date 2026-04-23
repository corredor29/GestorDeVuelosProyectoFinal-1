using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Entity;

public sealed class FlightReservationsEntityConfiguration : IEntityTypeConfiguration<FlightReservationsEntity>
{
    public void Configure(EntityTypeBuilder<FlightReservationsEntity> builder)
    {
        builder.ToTable("flight_reservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.BookingFlightId)
            .HasColumnName("booking_flight_id")
            .IsRequired();

        builder.HasIndex(x => x.BookingFlightId)
            .IsUnique();

        builder.HasOne<BookingFlightsEntity>()
            .WithMany()
            .HasForeignKey(x => x.BookingFlightId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
