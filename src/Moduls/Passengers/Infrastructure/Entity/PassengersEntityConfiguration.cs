using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;

public sealed class PassengersEntityConfiguration : IEntityTypeConfiguration<PassengersEntity>
{
    public void Configure(EntityTypeBuilder<PassengersEntity> builder)
    {
        builder.ToTable("passengers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();
        builder.Property(x => x.PassengerTypeId)
            .HasColumnName("passenger_type_id")
            .IsRequired();

        // Aplicamos la lógica de negocio: Una persona no puede ser dos pasajeros distintos
        builder.HasIndex(x => x.PersonId)
            .IsUnique();

        builder.HasOne(x => x.Person)
            .WithOne(p => p.Passenger)
            .HasForeignKey<PassengersEntity>(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PassengerType)
            .WithMany()
            .HasForeignKey(x => x.PassengerTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}