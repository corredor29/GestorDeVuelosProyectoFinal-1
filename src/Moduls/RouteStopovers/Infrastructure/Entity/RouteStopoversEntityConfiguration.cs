using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity;

public sealed class RouteStopoversEntityConfiguration : IEntityTypeConfiguration<RouteStopoversEntity>
{
    public void Configure(EntityTypeBuilder<RouteStopoversEntity> builder)
    {
        builder.ToTable("route_stopovers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RouteId)
            .HasColumnName("route_id")
            .IsRequired();

        builder.Property(x => x.StopoverAirportId)
            .HasColumnName("stopover_airport_id")
            .IsRequired();

        builder.Property(x => x.StopOrder)
            .HasColumnName("stop_order")
            .IsRequired();

        builder.Property(x => x.LayoverMin)
            .HasColumnName("layover_min")
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(x => new { x.RouteId, x.StopOrder })
            .IsUnique()
            .HasDatabaseName("uk_route_stopovers_route_stop_order");

        builder.HasOne(x => x.Route)
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_route_stopovers_route");

        builder.HasOne(x => x.StopoverAirport)
            .WithMany()
            .HasForeignKey(x => x.StopoverAirportId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_route_stopovers_stopover_airport");
    }
}
