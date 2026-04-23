using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;

public sealed class AirportAirlineEntityConfiguration : IEntityTypeConfiguration<AirportAirlineEntity>
{
    public void Configure(EntityTypeBuilder<AirportAirlineEntity> builder)
    {
        builder.ToTable("airport_airline");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.AirportId)
            .HasColumnName("airport_id")
            .IsRequired();

        builder.Property(x => x.AirlineId)
            .HasColumnName("airline_id")
            .IsRequired();

        builder.Property(x => x.Terminal)
            .HasColumnName("terminal")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.HasIndex(x => new { x.AirportId, x.AirlineId })
            .IsUnique();

        builder.HasOne(x => x.Airport)
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Airline)
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
