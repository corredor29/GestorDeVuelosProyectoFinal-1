using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;

public sealed class RegionEntityConfiguration : IEntityTypeConfiguration<RegionEntity>
{
    public void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
        builder.ToTable("regions");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Type)
            .HasColumnName("type")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(r => r.CountryId)
            .HasColumnName("country_id")
            .IsRequired();
    }
}