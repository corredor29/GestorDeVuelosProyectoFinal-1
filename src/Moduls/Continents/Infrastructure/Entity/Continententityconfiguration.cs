using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;

public sealed class ContinentEntityConfiguration : IEntityTypeConfiguration<ContinentEntity>
{
    public void Configure(EntityTypeBuilder<ContinentEntity> builder)
    {
        builder.ToTable("continents");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}