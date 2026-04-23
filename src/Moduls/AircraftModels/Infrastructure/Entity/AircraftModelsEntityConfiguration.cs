using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;

public sealed class AircraftModelsEntityConfiguration : IEntityTypeConfiguration<AircraftModelsEntity>
{
    public void Configure(EntityTypeBuilder<AircraftModelsEntity> builder)
    {
        builder.ToTable("aircraft_models");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(m => m.AircraftManufacturerId)
            .HasColumnName("manufacturer_id")
            .IsRequired();

        builder.Property(m => m.ModelName)
            .HasColumnName("model_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.MaxCapacity)
            .HasColumnName("max_capacity")
            .IsRequired();

        builder.Property(m => m.MaxTakeoffWeightKg)
            .HasColumnName("max_takeoff_weight_kg")
            .HasColumnType("decimal(10,2)");

        builder.Property(m => m.FuelConsumptionKgH)
            .HasColumnName("fuel_consumption_kg_h")
            .HasColumnType("decimal(8,2)");

        builder.Property(m => m.CruiseSpeedKmh)
            .HasColumnName("cruise_speed_kmh");

        builder.Property(m => m.CruiseAltitudeFt)
            .HasColumnName("cruise_altitude_ft");

        builder.HasIndex(m => new { m.AircraftManufacturerId, m.ModelName })
            .IsUnique();

        builder.HasOne(m => m.Manufacturer)
            .WithMany(m => m.Models)
            .HasForeignKey(m => m.AircraftManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
