using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;

public sealed class StaffEntityConfiguration : IEntityTypeConfiguration<StaffEntity>
{
    public void Configure(EntityTypeBuilder<StaffEntity> builder)
    {
        builder.ToTable("staff");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.PositionId)
            .HasColumnName("position_id")
            .IsRequired();

        builder.Property(x => x.AirlineId)
            .HasColumnName("airline_id")
            .IsRequired(false);

        builder.Property(x => x.AirportId)
            .HasColumnName("airport_id")
            .IsRequired(false);

        builder.Property(x => x.HireDate)
            .HasColumnName("hire_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(x => x.PersonId)
            .IsUnique();

        builder.HasOne(x => x.Person)
            .WithOne(p => p.StaffMember)
            .HasForeignKey<StaffEntity>(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Airline)
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Airport)
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
