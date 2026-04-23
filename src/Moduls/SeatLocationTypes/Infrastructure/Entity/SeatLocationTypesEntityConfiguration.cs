using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;

public sealed class SeatLocationTypesEntityConfiguration : IEntityTypeConfiguration<SeatLocationTypesEntity>
{
    public void Configure(EntityTypeBuilder<SeatLocationTypesEntity> builder)
    {
        builder.ToTable("seat_location_types");

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
    } 
}
