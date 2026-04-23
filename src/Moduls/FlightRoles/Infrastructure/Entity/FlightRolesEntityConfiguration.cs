using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;

public sealed class FlightRolesEntityConfiguration : IEntityTypeConfiguration<FlightRolesEntity>
{
    public void Configure(EntityTypeBuilder<FlightRolesEntity> builder)
    {
        builder.ToTable("flight_crew_roles");

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
