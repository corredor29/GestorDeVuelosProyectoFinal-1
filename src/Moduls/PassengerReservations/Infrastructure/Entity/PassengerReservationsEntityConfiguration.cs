// PassengerReservationsEntityConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;

public class PassengerReservationsEntityConfiguration : IEntityTypeConfiguration<PassengerReservationsEntity>
{
    public void Configure(EntityTypeBuilder<PassengerReservationsEntity> builder)
    {
        builder.ToTable("passenger_reservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Flight_Reservation_Id)
            .HasColumnName("flight_reservation_id")
            .IsRequired();

        builder.Property(x => x.Passenger_Id)
            .HasColumnName("passenger_id")
            .IsRequired();

        builder.HasIndex(x => new { x.Flight_Reservation_Id, x.Passenger_Id })
            .IsUnique();

        builder.HasOne(x => x.FlightReservation)
            .WithMany()
            .HasForeignKey(x => x.Flight_Reservation_Id)
            .HasConstraintName("fk_passenger_reservations_flight_reservation")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Passenger)
            .WithMany()
            .HasForeignKey(x => x.Passenger_Id)
            .HasConstraintName("fk_passenger_reservations_passenger")
            .OnDelete(DeleteBehavior.Restrict);
    }
}