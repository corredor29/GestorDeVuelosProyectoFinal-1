using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;

public class BaggageTypesEntityConfiguration : IEntityTypeConfiguration<BaggageTypesEntity>
{
    public void Configure(EntityTypeBuilder<BaggageTypesEntity> builder)
    {
        builder.ToTable("baggage_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.MaxWeightKg)
            .HasColumnName("max_weight_kg")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(x => x.BasePrice)
            .HasColumnName("base_price")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}