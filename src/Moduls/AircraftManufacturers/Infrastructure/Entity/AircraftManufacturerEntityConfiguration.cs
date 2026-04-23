using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Entity;

public sealed class AircraftManufacturerEntityConfiguration : IEntityTypeConfiguration<AircraftManufacturerEntity>
{
    public void Configure(EntityTypeBuilder<AircraftManufacturerEntity> builder)
    {
        builder.ToTable("aircraft_manufacturers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
