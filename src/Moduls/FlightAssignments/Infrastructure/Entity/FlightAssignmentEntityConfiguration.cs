using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;

public sealed class FlightAssignmentEntityConfiguration : IEntityTypeConfiguration<FlightAssignmentEntity>
{
    public void Configure(EntityTypeBuilder<FlightAssignmentEntity> builder)
    {
        builder.ToTable("flight_crew_assignments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FlightId)
            .HasColumnName("flight_id")
            .IsRequired();

        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();

        builder.Property(x => x.FlightRoleId)
            .HasColumnName("flight_role_id")
            .IsRequired();

        builder.HasIndex(x => new { x.FlightId, x.StaffId })
            .IsUnique();

        builder.HasOne(x => x.Flight)
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FlightRoles)
            .WithMany()
            .HasForeignKey(x => x.FlightRoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
