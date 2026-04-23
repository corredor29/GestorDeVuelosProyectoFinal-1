using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;

public sealed class AirlineEntityConfiguration : IEntityTypeConfiguration<AirlineEntity>
{
    public void Configure(EntityTypeBuilder<AirlineEntity> builder)
    {
        builder.ToTable("airlines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.IataCode)
            .HasColumnName("iata_code")
            .HasMaxLength(3)
            .IsRequired();

        builder.HasIndex(x => x.IataCode)
            .IsUnique();

        builder.Property(x => x.OriginCountryId)
            .HasColumnName("origin_country_id")
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

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.OriginCountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
