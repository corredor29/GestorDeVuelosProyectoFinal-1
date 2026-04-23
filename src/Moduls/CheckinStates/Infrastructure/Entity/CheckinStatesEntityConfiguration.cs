using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;

public class CheckinStatesEntityConfiguration : IEntityTypeConfiguration<CheckinStatesEntity>
{
    public void Configure(EntityTypeBuilder<CheckinStatesEntity> builder)
    {
        builder.ToTable("checkin_states");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();
    }
}