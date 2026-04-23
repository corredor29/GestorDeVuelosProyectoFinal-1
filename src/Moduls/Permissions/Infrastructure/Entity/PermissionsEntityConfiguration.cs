using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Entity;

public class PermissionsEntityConfiguration : IEntityTypeConfiguration<PermissionsEntity>
{
    public void Configure(EntityTypeBuilder<PermissionsEntity> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}