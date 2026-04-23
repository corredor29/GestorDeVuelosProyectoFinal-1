using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;

public class TicketStatesEntityConfiguration : IEntityTypeConfiguration<TicketStatesEntity>
{
    public void Configure(EntityTypeBuilder<TicketStatesEntity> builder)
    {
        builder.ToTable("ticket_states");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
    }
}