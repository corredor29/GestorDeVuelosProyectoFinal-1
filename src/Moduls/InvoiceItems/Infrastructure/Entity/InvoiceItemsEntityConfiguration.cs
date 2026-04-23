using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;

public class InvoiceItemsEntityConfiguration : IEntityTypeConfiguration<InvoiceItemsEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemsEntity> builder)
    {
        builder.ToTable("invoice_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Invoice_Id)
            .HasColumnName("invoice_id")
            .IsRequired();

        builder.Property(x => x.Item_Type_Id)
            .HasColumnName("item_type_id")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.BookingPassenger_Id)
            .HasColumnName("booking_passenger_id")
            .IsRequired(false);

        builder.HasOne(x => x.Invoice)
            .WithMany(x => x.InvoiceItems)
            .HasForeignKey(x => x.Invoice_Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ItemType)
            .WithMany(x => x.InvoiceItems)
            .HasForeignKey(x => x.Item_Type_Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
