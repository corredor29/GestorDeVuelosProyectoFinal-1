using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// Asegúrate de que este using coincida con la ubicación de tu StreetTypeEntity
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;

public sealed class StreetTypeEntityConfiguration : IEntityTypeConfiguration<StreetTypeEntity>
{
    public void Configure(EntityTypeBuilder<StreetTypeEntity> builder)
    {
        // Nombre de la tabla en tu DB
        builder.ToTable("street_types");

        // Clave primaria
        builder.HasKey(x => x.Id);

        // Configuración del ID (Auto-incremento para int)
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Configuración del Nombre
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
    }
}