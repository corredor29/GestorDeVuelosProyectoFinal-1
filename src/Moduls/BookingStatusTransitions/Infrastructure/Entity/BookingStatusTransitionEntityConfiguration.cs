using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Entity;

public sealed class BookingStatusTransitionEntityConfiguration : IEntityTypeConfiguration<BookingStatusTransitionEntity>
{
    public void Configure(EntityTypeBuilder<BookingStatusTransitionEntity> builder)
    {
        builder.ToTable("booking_status_transitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FromStatusId)
            .HasColumnName("from_status_id")
            .IsRequired();

        builder.Property(x => x.ToStatusId)
            .HasColumnName("to_status_id")
            .IsRequired();

        builder.HasIndex(x => new { x.FromStatusId, x.ToStatusId })
            .IsUnique();

        builder.HasOne(x => x.FromStatus)
            .WithMany()
            .HasForeignKey(x => x.FromStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToStatus)
            .WithMany()
            .HasForeignKey(x => x.ToStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
