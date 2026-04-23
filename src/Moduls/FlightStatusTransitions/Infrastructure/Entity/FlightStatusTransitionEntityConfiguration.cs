using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;

public sealed class FlightStatusTransitionEntityConfiguration : IEntityTypeConfiguration<FlightStatusTransitionEntity>
{
    public void Configure(EntityTypeBuilder<FlightStatusTransitionEntity> builder)
    {
        builder.ToTable("flight_status_transitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FromStatusId)
            .HasColumnName("from_status_id")
            .IsRequired();

        builder.Property(x => x.ToStatusId)
            .HasColumnName("to_status_id")
            .IsRequired();

        builder.HasIndex(x => new { x.FromStatusId, x.ToStatusId })
            .IsUnique();

        builder.HasOne(x => x.FromStatus)
            .WithMany()
            .HasForeignKey(x => x.FromStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToStatus)
            .WithMany()
            .HasForeignKey(x => x.ToStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
