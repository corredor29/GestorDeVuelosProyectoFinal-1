using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Repository;

public sealed class BookingsRepository : IBookingsRepository
{
    private readonly AppDbContext _context;

    public BookingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Booking>> GetByClientIdAsync(CustomersId clientId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Bookings
            .AsNoTracking()
            .Where(x => x.ClientId == clientId.Value)
            .OrderByDescending(x => x.BookedAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Booking>> GetByStatusIdAsync(BookingStatusesId statusId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Bookings
            .AsNoTracking()
            .Where(x => x.BookingStatusId == statusId.Value)
            .OrderByDescending(x => x.BookedAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Booking>> GetByBookedAtRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Bookings
            .AsNoTracking()
            .Where(x => x.BookedAt >= from && x.BookedAt <= to)
            .OrderBy(x => x.BookedAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Bookings
            .AsNoTracking()
            .OrderByDescending(x => x.BookedAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await _context.Bookings.AddAsync(MapToEntity(booking), cancellationToken);
    }

    public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        if (booking.Id is null)
            throw new InvalidOperationException("No se puede actualizar una reserva sin id.");

        var entity = await _context.Bookings
            .FirstOrDefaultAsync(x => x.Id == booking.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro la reserva con id {booking.Id.Value}.");

        entity.ClientId = booking.ClientId.Value;
        entity.BookedAt = booking.BookedAt.Value;
        entity.BookingStatusId = booking.BookingStatusId.Value;
        entity.TotalAmount = booking.TotalAmount.Value;
        entity.ExpiresAt = booking.ExpiresAt.Value;
        entity.CreatedAt = booking.CreatedAt.Value;
        entity.UpdatedAt = booking.UpdatedAt.Value;
    }

    public async Task DeleteByIdAsync(BookingId id, CancellationToken cancellationToken = default)
    {
        await _context.Bookings
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteByClientIdAsync(CustomersId clientId, CancellationToken cancellationToken = default)
    {
        await _context.Bookings
            .Where(x => x.ClientId == clientId.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static Booking MapToDomain(BookingEntity entity)
        => Booking.FromPrimitives(
            entity.Id,
            entity.ClientId,
            entity.BookedAt,
            entity.BookingStatusId,
            entity.TotalAmount,
            entity.ExpiresAt,
            entity.CreatedAt,
            entity.UpdatedAt);

    private static BookingEntity MapToEntity(Booking booking)
    {
        return new BookingEntity
        {
            ClientId = booking.ClientId.Value,
            BookedAt = booking.BookedAt.Value,
            BookingStatusId = booking.BookingStatusId.Value,
            TotalAmount = booking.TotalAmount.Value,
            ExpiresAt = booking.ExpiresAt.Value,
            CreatedAt = booking.CreatedAt.Value,
            UpdatedAt = booking.UpdatedAt.Value
        };
    }
}
