using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;

public sealed class AirportEntityConfiguration : IEntityTypeConfiguration<AirportEntity>
{
    public void Configure(EntityTypeBuilder<AirportEntity> builder)
    {
        builder.ToTable("airports");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.IataCode)
            .HasColumnName("iata_code")
            .HasMaxLength(3)
            .IsRequired();

        builder.HasIndex(x => x.IataCode)
            .IsUnique();

        builder.Property(x => x.IcaoCode)
            .HasColumnName("icao_code")
            .HasMaxLength(4)
            .IsRequired(false);

        builder.HasIndex(x => x.IcaoCode)
            .IsUnique();

        builder.Property(x => x.CityId)
            .HasColumnName("city_id")
            .IsRequired();

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
