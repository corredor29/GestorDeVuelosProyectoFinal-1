using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;

public sealed class FlightEntityConfiguration : IEntityTypeConfiguration<FlightEntity>
{
    public void Configure(EntityTypeBuilder<FlightEntity> builder)
    {
        builder.ToTable("flights");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FlightCode)
            .HasColumnName("flight_code")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(x => x.FlightCode)
            .IsUnique();

        builder.Property(x => x.AirlineId).HasColumnName("airline_id");
        builder.Property(x => x.RouteId).HasColumnName("route_id");
        builder.Property(x => x.AircraftId).HasColumnName("aircraft_id");
        builder.Property(x => x.DepartureAt).HasColumnName("departure_at");
        builder.Property(x => x.EstimatedArrivalAt).HasColumnName("estimated_arrival_at");
        builder.Property(x => x.TotalCapacity).HasColumnName("total_capacity");
        builder.Property(x => x.AvailableSeats).HasColumnName("available_seats");
        builder.Property(x => x.FlightStatusId).HasColumnName("flight_status_id");
        builder.Property(x => x.RescheduledAt).HasColumnName("rescheduled_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(x => x.Airline)
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Route)
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Aircraft)
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FlightStatus)
            .WithMany()
            .HasForeignKey(x => x.FlightStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
