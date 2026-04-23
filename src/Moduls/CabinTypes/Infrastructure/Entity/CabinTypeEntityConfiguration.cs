using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;

public sealed class CabinTypeEntityConfiguration : IEntityTypeConfiguration<CabinTypeEntity>
{
    public void Configure(EntityTypeBuilder<CabinTypeEntity> builder)
    {
        builder.ToTable("cabin_types");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}