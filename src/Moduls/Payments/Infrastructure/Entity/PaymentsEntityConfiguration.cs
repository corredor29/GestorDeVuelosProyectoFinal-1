using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;

public class PaymentsEntityConfiguration : IEntityTypeConfiguration<PaymentsEntity>
{
    public void Configure(EntityTypeBuilder<PaymentsEntity> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.PaidAt)
            .HasColumnName("paid_at")
            .IsRequired();

        builder.Property(x => x.PaymentStatusId)
            .HasColumnName("payment_status_id")
            .IsRequired();

        builder.Property(x => x.PaymentMethodId)
            .HasColumnName("payment_method_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.BookingId)
            .HasConstraintName("fk_payments_bookings")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.PaymentStatus)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.PaymentStatusId)
            .HasConstraintName("fk_payments_payment_statuses")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PaymentMethod)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.PaymentMethodId)
            .HasConstraintName("fk_payments_payment_methods")
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}