using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;

public class TicketEntityConfiguration : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.IssueDate)
            .HasColumnName("issue_date")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(x => x.PassengerReservation_Id)
            .HasColumnName("passenger_reservation_id")
            .IsRequired();

        builder.Property(x => x.TicketState_Id)
            .HasColumnName("ticket_state_id")
            .IsRequired();

        builder.HasOne(x => x.PassengerReservation)
            .WithMany()
            .HasForeignKey(x => x.PassengerReservation_Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TicketState)
            .WithMany()
            .HasForeignKey(x => x.TicketState_Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}