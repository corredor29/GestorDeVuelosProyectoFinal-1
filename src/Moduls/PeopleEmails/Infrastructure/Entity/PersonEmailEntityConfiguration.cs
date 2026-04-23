using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Entity;

public sealed class PersonEmailEntityConfiguration : IEntityTypeConfiguration<PersonEmailEntity>
{
    public void Configure(EntityTypeBuilder<PersonEmailEntity> builder)
    {
        builder.ToTable("person_emails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.EmailUser)
            .HasColumnName("email_user")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.EmailDomainId)
            .HasColumnName("email_domain_id")
            .IsRequired();

        builder.Property(x => x.IsPrimary)
            .HasColumnName("is_primary")
            .HasDefaultValue(false)
            .IsRequired();

        // Regla útil: evita duplicar exactamente el mismo email completo
        // (user + domain) para la misma persona.
        builder.HasIndex(x => new { x.PersonId, x.EmailUser, x.EmailDomainId })
            .IsUnique();

        builder.HasOne(x => x.Person)
            .WithMany(p => p.PersonEmails)
            .HasForeignKey(x => x.PersonId)
            .HasConstraintName("fk_person_emails_person")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EmailDomain)
            .WithMany()
            .HasForeignKey(x => x.EmailDomainId)
            .HasConstraintName("fk_person_emails_email_domain")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
