using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;

public sealed class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.RegionId)
            .HasColumnName("region_id")
            .IsRequired();

        builder.Property(c => c.RegionId1)
            .HasColumnName("RegionId1")
            .IsRequired();
            
        builder.HasOne(c => c.Region)
            .WithMany(r => r.Cities)
            .HasForeignKey(c => c.RegionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
