using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Entities;

public sealed class SeasonEntityConfiguration : IEntityTypeConfiguration<SeasonEntity>
{
    public void Configure(EntityTypeBuilder<SeasonEntity> builder)
    {
        builder.ToTable("seasons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.PriceFactor)
            .HasColumnName("price_factor")
            .HasColumnType("decimal(5,4)")
            .HasDefaultValue(1.0000m)
            .IsRequired();
    }
}
