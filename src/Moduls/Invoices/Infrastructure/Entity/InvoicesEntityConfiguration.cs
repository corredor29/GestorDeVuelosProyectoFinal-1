using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;

public class InvoicesEntityConfiguration : IEntityTypeConfiguration<InvoicesEntity>
{
    public void Configure(EntityTypeBuilder<InvoicesEntity> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Booking_Id)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(x => x.InvoiceNumber)
            .HasColumnName("invoice_number")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.IssuedAt)
            .HasColumnName("issued_at")
            .IsRequired();

        builder.Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Taxes)
            .HasColumnName("taxes")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Total)
            .HasColumnName("total")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.Booking_Id)
            .HasConstraintName("fk_invoices_booking")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.Booking_Id).IsUnique();
        builder.HasIndex(x => x.InvoiceNumber).IsUnique();
    }
}
