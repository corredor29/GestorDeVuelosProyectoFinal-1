using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;

public class PaymentMediumTypesEntityConfiguration : IEntityTypeConfiguration<PaymentMediumTypesEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMediumTypesEntity> builder)
    {
        builder.ToTable("payment_method_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}