using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;

public sealed class AvailabilityStateEntityConfiguration : IEntityTypeConfiguration<AvailabilityStateEntity>
{
    public void Configure(EntityTypeBuilder<AvailabilityStateEntity> builder)
    {
        builder.ToTable("availability_statuses");

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
