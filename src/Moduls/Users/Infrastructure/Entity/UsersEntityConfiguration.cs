using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;

public class UsersEntityConfiguration : IEntityTypeConfiguration<UsersEntity>
{
    public void Configure(EntityTypeBuilder<UsersEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Person_Id)
            .HasColumnName("person_id")
            .IsRequired(false);

        builder.Property(x => x.Role_Id)
            .HasColumnName("role_id")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.LastAccess)
            .HasColumnName("last_access")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.Person_Id).IsUnique();
        builder.HasOne(x => x.Person)
            .WithOne(p => p.User)
            .HasForeignKey<UsersEntity>(x => x.Person_Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.Role_Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}