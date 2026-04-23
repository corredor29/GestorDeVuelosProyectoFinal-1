using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;

public sealed class FlightStatusEntityConfiguration : IEntityTypeConfiguration<FlightStatusEntity>
{
    public void Configure(EntityTypeBuilder<FlightStatusEntity> builder)
    {
        builder.ToTable("flight_statuses");

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
