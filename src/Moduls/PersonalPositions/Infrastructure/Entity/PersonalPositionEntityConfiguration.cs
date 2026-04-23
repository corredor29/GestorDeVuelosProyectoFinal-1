using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;

public sealed class PersonalPositionEntityConfiguration : IEntityTypeConfiguration<PersonalPositionEntity>
{
    public void Configure(EntityTypeBuilder<PersonalPositionEntity> builder)
    {
        builder.ToTable("staff_positions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
