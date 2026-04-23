using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;

public class PaymentMethodsEntityConfiguration : IEntityTypeConfiguration<PaymentMethodsEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMethodsEntity> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.PaymentMethodTypeId)
            .HasColumnName("payment_method_type_id")
            .IsRequired();

        builder.Property(x => x.CardTypeId)
            .HasColumnName("card_type_id")
            .IsRequired(false);

        builder.Property(x => x.CardIssuerId)
            .HasColumnName("card_issuer_id")
            .IsRequired(false);

        builder.Property(x => x.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(50)
            .IsRequired();
        builder.HasOne(x => x.PaymentMethodType)
            .WithMany(x => x.PaymentMethods)
            .HasForeignKey(x => x.PaymentMethodTypeId)
            .HasConstraintName("fk_payment_methods_payment_medium_types")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CardType)
            .WithMany(x => x.PaymentMethods)
            .HasForeignKey(x => x.CardTypeId)
            .HasConstraintName("fk_payment_methods_card_types")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CardIssuer)
            .WithMany(x => x.PaymentMethods)
            .HasForeignKey(x => x.CardIssuerId)
            .HasConstraintName("fk_payment_methods_card_issuers")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.DisplayName).IsUnique();
    }
}