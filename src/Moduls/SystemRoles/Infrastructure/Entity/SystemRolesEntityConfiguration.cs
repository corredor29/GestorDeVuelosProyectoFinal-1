using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;

public class SystemRolesEntityConfiguration : IEntityTypeConfiguration<SystemRolesEntity>
{
    public void Configure(EntityTypeBuilder<SystemRolesEntity> builder)
    {
        builder.ToTable("SystemRoles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(
            new SystemRolesEntity { Id = 1, Name = "Admin", Description = "Administrador del sistema" }
        );
    }
}