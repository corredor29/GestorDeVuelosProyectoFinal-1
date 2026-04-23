using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;

public sealed class PeoplePhoneEntityConfiguration : IEntityTypeConfiguration<PeoplePhoneEntity>
{
    public void Configure(EntityTypeBuilder<PeoplePhoneEntity> builder)
    {
        builder.ToTable("person_phones");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.PhoneCodeId)
            .HasColumnName("phone_code_id")
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired();

        builder.HasOne(x => x.Person)
            .WithMany(p => p.PersonPhones)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PhoneCode)
            .WithMany(p => p.PeoplePhone)
            .HasForeignKey(x => x.PhoneCodeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
