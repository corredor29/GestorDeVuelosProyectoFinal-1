using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;

public sealed class PassengerTypeEntityConfiguration : IEntityTypeConfiguration<PassengerTypeEntity>
{
    public void Configure(EntityTypeBuilder<PassengerTypeEntity> builder)
    {
        builder.ToTable("passenger_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.MinAge)
            .HasColumnName("min_age");

        builder.Property(x => x.MaxAge)
            .HasColumnName("max_age");

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
