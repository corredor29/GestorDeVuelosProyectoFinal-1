using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Entity;

public sealed class CabinConfigurationEntityConfiguration : IEntityTypeConfiguration<CabinConfiurationEntity>
{
    public void Configure(EntityTypeBuilder<CabinConfiurationEntity> builder)
    {
        builder.ToTable("cabin_configurations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.AircraftId)
            .HasColumnName("aircraft_id")
            .IsRequired();

        builder.Property(x => x.CabinTypeId)
            .HasColumnName("cabin_type_id")
            .IsRequired();

        builder.Property(x => x.RowStart)
            .HasColumnName("row_start")
            .IsRequired();

        builder.Property(x => x.RowEnd)
            .HasColumnName("row_end")
            .IsRequired();

        builder.Property(x => x.SeatsPerRow)
            .HasColumnName("seats_per_row").IsRequired();

        builder.Property(x => x.SeatLetters)
            .HasColumnName("seat_letters").HasMaxLength(10).IsRequired();

        // Regla de negocio: para este módulo guía dejamos una configuración por
        // combinación (aircraft, cabin_type). Si luego tu modelo necesita varias
        // secciones del mismo tipo de cabina en el mismo avión, aquí tendrías que
        // rediseñar este índice único.
        builder.HasIndex(x => new {
            x.AircraftId, x.CabinTypeId })
            .IsUnique();

        // Relación 1:N con aircraft.
        // Un aircraft puede tener muchas cabin configurations.
        // La FK física es aircraft_id -> aircraft.id
        builder.HasOne(x => x.Aircraft)
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .HasConstraintName("fk_cabin_configuration_aircraft")
            .OnDelete(DeleteBehavior.Restrict);

        // Relación 1:N con cabin_types.
        // Un cabin type puede aparecer en muchas cabin configurations.
        // La FK física es cabin_type_id -> cabin_types.id
        builder.HasOne(x => x.CabinType)
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .HasConstraintName("fk_cabin_configuration_cabin_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
