using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Auth.Infrastructure.Persistence;

// Esta configuración deja explícito cómo se mapea AppUserEntity a la tabla app_users.
public sealed class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUserEntity>
{
    public void Configure(EntityTypeBuilder<AppUserEntity> b)
    {
        b.ToTable("app_users");
        b.HasKey(x => x.Id);
        b.Property(x => x.Username).HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.Username).IsUnique();
        b.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
        b.Property(x => x.Role).HasMaxLength(20).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();
    }
}
