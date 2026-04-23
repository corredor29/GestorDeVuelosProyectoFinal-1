using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;

public sealed class FlightAssignmentStaffReferenceEntityConfiguration : IEntityTypeConfiguration<FlightAssignmentStaffReferenceEntity>
{
    public void Configure(EntityTypeBuilder<FlightAssignmentStaffReferenceEntity> builder)
    {
        builder.ToTable("flight_assignments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.FlightId)
            .HasColumnName("flight_id");

        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id");

        builder.HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
