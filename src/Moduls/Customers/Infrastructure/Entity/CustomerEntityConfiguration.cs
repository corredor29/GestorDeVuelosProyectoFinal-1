using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;

public sealed class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(x => x.PersonId)
            .IsUnique();

        // Relación 1:1 lógica desde negocio:
        // una persona solo puede convertirse en cliente una vez.
        // FK física: person_id -> persons.id
        builder.HasOne(x => x.Person)
            .WithOne(p => p.Client)
            .HasForeignKey<CustomerEntity>(x => x.PersonId)
            .HasConstraintName("fk_clients_person")
            .OnDelete(DeleteBehavior.Restrict);

        // Nota guía:
        // La validación "no eliminar si tiene reservas activas" debe conectarse cuando
        // exista en el proyecto la relación real entre reservations y clients/customers.
    }
}
