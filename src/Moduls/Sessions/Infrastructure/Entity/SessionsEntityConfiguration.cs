using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Infrastructure.Entity;

public class SessionsEntityConfiguration : IEntityTypeConfiguration<SessionsEntity>
{
    public void Configure(EntityTypeBuilder<SessionsEntity> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.User_Id)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.StartedAt)
            .HasColumnName("started_at")
            .IsRequired();

        builder.Property(x => x.EndedAt)
            .HasColumnName("ended_at")
            .IsRequired(false);

        builder.Property(x => x.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasOne<UsersEntity>()
            .WithMany()
            .HasForeignKey(x => x.User_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
