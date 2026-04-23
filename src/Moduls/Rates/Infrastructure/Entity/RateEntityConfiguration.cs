using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;

public sealed class RateEntityConfiguration : IEntityTypeConfiguration<RateEntity>
{
    public void Configure(EntityTypeBuilder<RateEntity> builder)
    {
        builder.ToTable("fares", table =>
        {
            table.HasCheckConstraint("chk_base_price", "base_price >= 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RouteId)
            .HasColumnName("route_id")
            .IsRequired();

        builder.Property(x => x.CabinTypeId)
            .HasColumnName("cabin_type_id")
            .IsRequired();

        builder.Property(x => x.PassengerTypeId)
            .HasColumnName("passenger_type_id")
            .IsRequired();

        builder.Property(x => x.SeasonId)
            .HasColumnName("season_id")
            .IsRequired();

        builder.Property(x => x.BasePrice)
            .HasColumnName("base_price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.ValidFrom)
            .HasColumnName("valid_from")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(x => x.ValidUntil)
            .HasColumnName("valid_until")
            .HasColumnType("date")
            .IsRequired(false);

        builder.HasIndex(x => new { x.RouteId, x.CabinTypeId, x.PassengerTypeId, x.SeasonId });
        builder.HasIndex(x => x.RouteId);
        builder.HasIndex(x => x.CabinTypeId);

        builder.HasOne(x => x.Route)
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CabinType)
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PassengerType)
            .WithMany()
            .HasForeignKey(x => x.PassengerTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Season)
            .WithMany()
            .HasForeignKey(x => x.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
