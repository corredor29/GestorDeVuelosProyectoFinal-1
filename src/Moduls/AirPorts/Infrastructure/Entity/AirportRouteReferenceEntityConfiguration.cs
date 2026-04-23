using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;

public sealed class AirportRouteReferenceEntityConfiguration : IEntityTypeConfiguration<AirportRouteReferenceEntity>
{
    public void Configure(EntityTypeBuilder<AirportRouteReferenceEntity> builder)
    {
        builder.ToTable("routes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.OriginAirportId)
            .HasColumnName("origin_airport_id");

        builder.Property(x => x.DestinationAirportId)
            .HasColumnName("destination_airport_id");
    }
}
