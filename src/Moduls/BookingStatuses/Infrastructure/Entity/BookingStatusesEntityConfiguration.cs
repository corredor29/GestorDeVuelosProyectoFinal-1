using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;

public sealed class BookingStatusesEntityConfiguration : IEntityTypeConfiguration<BookingStatusesEntity>
{
    public void Configure(EntityTypeBuilder<BookingStatusesEntity> builder)
    {
        builder.ToTable("booking_statuses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Name).IsUnique();
    }
}