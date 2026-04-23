using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;

public sealed class AircraftEntityConfiguration : IEntityTypeConfiguration<AircraftEntity>
{
    public void Configure(EntityTypeBuilder<AircraftEntity> builder)
    {
        builder.ToTable("aircraft");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.AircraftModelId)
            .HasColumnName("model_id")
            .IsRequired();

        builder.Property(x => x.AirlinesId)
            .HasColumnName("airline_id")
            .IsRequired();

        builder.Property(x => x.Registration)
            .HasColumnName("registration")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.Registration)
            .IsUnique();

        builder.Property(x => x.DateManufactured)
            .HasColumnName("manufactured_date")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.AircraftModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Airline)
            .WithMany()
            .HasForeignKey(x => x.AirlinesId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
