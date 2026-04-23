using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;

public sealed class EmailDomainsEntityConfiguration : IEntityTypeConfiguration<EmailDomainsEntity>
{
    public void Configure(EntityTypeBuilder<EmailDomainsEntity> builder)
    {
        builder.ToTable("email_domains");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Domain)
            .HasColumnName("domain")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Domain)
            .IsUnique();
    }
}
