using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;

public class InvoiceItemTypesEntityConfiguration : IEntityTypeConfiguration<InvoiceItemTypesEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemTypesEntity> builder)
    {
        builder.ToTable("invoice_item_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}