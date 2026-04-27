using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Infrastructure.Entity;

public sealed class WaitingListEntityConfiguration : IEntityTypeConfiguration<WaitingListEntity>
{
    public void Configure(EntityTypeBuilder<WaitingListEntity> builder)
    {
        builder.ToTable("waiting_list");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(x => x.FlightId)
            .HasColumnName("flight_id")
            .IsRequired();

        builder.Property(x => x.RequestedAt)
            .HasColumnName("requested_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasColumnName("priority")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(x => x.Booking)
            .WithMany(b => b.WaitingLists)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Flight)
            .WithMany(f => f.WaitingLists)
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.FlightId);
        builder.HasIndex(x => x.BookingId);
        builder.HasIndex(x => new { x.BookingId, x.FlightId }).IsUnique();
    }
}