using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;

public sealed class AddressEntityConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.StreetTypeId)
            .HasColumnName("street_type_id")
            .IsRequired();

        builder.Property(x => x.StreetName)
            .HasColumnName("street_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Number)
            .HasColumnName("number")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.Complement)
            .HasColumnName("complement")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.CityId)
            .HasColumnName("city_id")
            .IsRequired();

        builder.Property(x => x.CityId1)
            .HasColumnName("CityId1")
            .IsRequired();

        builder.Property(x => x.StreetTypeId1)
            .HasColumnName("StreetTypeId1")
            .IsRequired();

        builder.Property(x => x.PostalCode)
            .HasColumnName("postal_code")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.HasOne(x => x.StreetType)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.StreetTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
