using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;

public sealed class RouteEntityConfiguration : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("routes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.OriginAirportId)
            .HasColumnName("origin_airport_id")
            .IsRequired();

        builder.Property(x => x.DestinationAirportId)
            .HasColumnName("destination_airport_id")
            .IsRequired();

        builder.Property(x => x.DistanceKm)
            .HasColumnName("distance_km")
            .IsRequired(false);

        builder.Property(x => x.EstimatedDurationMin)
            .HasColumnName("estimated_duration_min")
            .IsRequired(false);

        builder.HasIndex(x => new { x.OriginAirportId, x.DestinationAirportId })
            .IsUnique();

        builder.HasOne(x => x.OriginAirport)
            .WithMany(a => a.OriginRoutes)
            .HasForeignKey(x => x.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DestinationAirport)
            .WithMany(a => a.DestinationRoutes)
            .HasForeignKey(x => x.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
