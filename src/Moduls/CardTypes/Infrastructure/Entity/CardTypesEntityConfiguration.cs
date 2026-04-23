using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Entity;

public class CardTypesEntityConfiguration : IEntityTypeConfiguration<CardTypesEntity>
{
    public void Configure(EntityTypeBuilder<CardTypesEntity> builder)
    {
        builder.ToTable("card_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}