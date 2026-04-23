using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;

public class BaggageEntityConfiguration : IEntityTypeConfiguration<BaggageEntity>
{
    public void Configure(EntityTypeBuilder<BaggageEntity> builder)
    {
        builder.ToTable("baggage");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.CheckinId)
            .HasColumnName("checkin_id")
            .IsRequired();

        builder.Property(x => x.BaggageTypeId)
            .HasColumnName("baggage_type_id")
            .IsRequired();

        builder.Property(x => x.WeightKg)
            .HasColumnName("weight_kg")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(x => x.ChargedPrice)
            .HasColumnName("charged_price")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();
        builder.HasOne(x => x.Checkin)
            .WithMany(x => x.Baggage)
            .HasForeignKey(x => x.CheckinId)
            .HasConstraintName("fk_baggage_checkin")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.BaggageType)
            .WithMany(x => x.Baggage)
            .HasForeignKey(x => x.BaggageTypeId)
            .HasConstraintName("fk_baggage_baggage_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}