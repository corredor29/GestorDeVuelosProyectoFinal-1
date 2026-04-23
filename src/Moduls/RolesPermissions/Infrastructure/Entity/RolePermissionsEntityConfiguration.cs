using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Entity;

public class RolePermissionsEntityConfiguration : IEntityTypeConfiguration<RolePermissionsEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionsEntity> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Role_Id)
            .HasColumnName("role_id")
            .IsRequired();

        builder.Property(x => x.Permission_Id)
            .HasColumnName("permission_id")
            .IsRequired();

        builder.HasIndex(x => new { x.Role_Id, x.Permission_Id })
            .IsUnique();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.Role_Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.Permission_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
