using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;

public sealed class BookingEntityConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("bookings", table =>
        {
            table.HasCheckConstraint("chk_total_amount", "total_amount >= 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(x => x.BookedAt)
            .HasColumnName("booked_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.BookingStatusId)
            .HasColumnName("booking_status_id")
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasIndex(x => x.ClientId);
        builder.HasIndex(x => x.BookingStatusId);
        builder.HasIndex(x => x.BookedAt);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(x => x.ClientId)
            .HasConstraintName("fk_bookings_client")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BookingStatus)
            .WithMany()
            .HasForeignKey(x => x.BookingStatusId)
            .HasConstraintName("fk_bookings_booking_status")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
