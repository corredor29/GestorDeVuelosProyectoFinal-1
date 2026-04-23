using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;

public class CheckinEntityConfiguration : IEntityTypeConfiguration<CheckinEntity>
{
    public void Configure(EntityTypeBuilder<CheckinEntity> builder)
    {
        builder.ToTable("check_ins");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.TicketId)
            .HasColumnName("ticket_id")
            .IsRequired();

        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();

        builder.Property(x => x.FlightSeatId)
            .HasColumnName("flight_seat_id")
            .IsRequired();

        builder.Property(x => x.CheckedInAt)
            .HasColumnName("checked_in_at")
            .IsRequired();

        builder.Property(x => x.CheckinStatusId)
            .HasColumnName("checkin_status_id")
            .IsRequired();

        builder.Property(x => x.BoardingPassNumber)
            .HasColumnName("boarding_pass_number")
            .HasMaxLength(20)
            .IsRequired();
        builder.HasOne(x => x.Ticket)
            .WithMany(x => x.Checkins)
            .HasForeignKey(x => x.TicketId)
            .HasConstraintName("fk_checkins_ticket")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .HasConstraintName("fk_checkins_staff")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CheckinStatus)
            .WithMany()
            .HasForeignKey(x => x.CheckinStatusId)
            .HasConstraintName("fk_checkins_checkin_status")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FlightSeat)
            .WithMany(x => x.Checkins)
            .HasForeignKey(x => x.FlightSeatId)
            .HasConstraintName("fk_checkins_flight_seat")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.TicketId).IsUnique();
        builder.HasIndex(x => x.FlightSeatId).IsUnique();
        builder.HasIndex(x => x.BoardingPassNumber).IsUnique();
    }
}