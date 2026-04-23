using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Entity;

public class CardIssuerEntityConfiguration : IEntityTypeConfiguration<CardIssuerEntity>
{
    public void Configure(EntityTypeBuilder<CardIssuerEntity> builder)
    {
        builder.ToTable("card_issuers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IssuerNumber)
            .HasColumnName("issuer_number")
            .HasMaxLength(15)
            .IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}