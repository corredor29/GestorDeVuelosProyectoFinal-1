using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;

public sealed class FlightSeatEntityConfiguration : IEntityTypeConfiguration<FlightSeatEntity>
{
    public void Configure(EntityTypeBuilder<FlightSeatEntity> builder)
    {
        builder.ToTable("flight_seats");        

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.FlightId)
            .HasColumnName("flight_id")
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.CabinTypeId)
            .HasColumnName("cabin_type_id")
            .IsRequired();

        builder.Property(x => x.SeatLocationTypeId)
            .HasColumnName("seat_location_type_id")
            .IsRequired();

        builder.Property(x => x.IsOccupied)
            .HasColumnName("is_occupied")
            .IsRequired();

        builder.HasOne(x => x.Flight)
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CabinType)
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SeatLocationType)
            .WithMany()
            .HasForeignKey(x => x.SeatLocationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
