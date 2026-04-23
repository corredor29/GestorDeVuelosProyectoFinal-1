using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;

public class PaymentStatusesEntityConfiguration : IEntityTypeConfiguration<PaymentStatusesEntity>
{
    public void Configure(EntityTypeBuilder<PaymentStatusesEntity> builder)
    {
        builder.ToTable("payment_statuses");

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