using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;

public sealed class CountryEntityConfiguration : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder.ToTable("countries");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.IsoCode)
            .HasColumnName("iso_code")
            .HasMaxLength(3)
            .IsRequired();

        builder.HasIndex(c => c.IsoCode)
            .IsUnique();

        builder.Property(c => c.ContinentId)
            .HasColumnName("continent_id")
            .IsRequired();

        builder.HasOne(c => c.Continent)
            .WithMany(co => co.Countries)
            .HasForeignKey(c => c.ContinentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}