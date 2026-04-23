using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;

public sealed class PhoneCodeEntityConfiguration : IEntityTypeConfiguration<PhoneCodeEntity>
{
    public void Configure(EntityTypeBuilder<PhoneCodeEntity> builder)
    {
        builder.ToTable("phone_codes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CountryCode)
            .HasColumnName("country_code")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.CountryName)
            .HasColumnName("country_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.CountryCode)
            .IsUnique();
    }
}
