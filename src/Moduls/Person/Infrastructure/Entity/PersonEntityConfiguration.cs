using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;

public sealed class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.ToTable("persons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.DocumentTypeId)
            .HasColumnName("document_type_id")
            .IsRequired();

        builder.Property(x => x.DocumentNumber)
            .HasColumnName("document_number")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.BirthDate)
            .HasColumnName("birth_date")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(x => x.Gender)
            .HasColumnName("gender")
            .HasColumnType("char(1)")
            .IsRequired(false);

        builder.Property(x => x.AddressId)
            .HasColumnName("address_id")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(x => new { x.DocumentTypeId, x.DocumentNumber })
            .IsUnique();

        builder.HasOne(x => x.DocumentType)
            .WithMany()
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
