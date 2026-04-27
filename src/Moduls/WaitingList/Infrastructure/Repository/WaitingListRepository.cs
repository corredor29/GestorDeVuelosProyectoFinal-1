using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Infrastructure.Repository;

public sealed class WaitingListRepository : IWaitingListRepository
{
    private readonly AppDbContext _context;

    public WaitingListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WaitingLis?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.WaitingLists
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<WaitingLis>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.WaitingLists
            .AsNoTracking()
            .Where(x => x.FlightId == flightId)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.RequestedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<WaitingLis>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.WaitingLists
            .AsNoTracking()
            .Where(x => x.BookingId == bookingId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<bool> ExistsAsync(int bookingId, int flightId, CancellationToken cancellationToken = default)
    {
        return await _context.WaitingLists
            .AsNoTracking()
            .AnyAsync(x => x.BookingId == bookingId && x.FlightId == flightId && x.Status == "Waiting", cancellationToken);
    }

    public async Task<WaitingLis?> GetNextCandidateAsync(int flightId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.WaitingLists
            .AsNoTracking()
            .Where(x => x.FlightId == flightId && x.Status == "Waiting")
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.RequestedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(WaitingLis entry, CancellationToken cancellationToken = default)
    {
        await _context.WaitingLists.AddAsync(MapToEntity(entry), cancellationToken);
    }

    public async Task UpdateAsync(WaitingLis entry, CancellationToken cancellationToken = default)
    {
        if (entry.Id is null)
            throw new InvalidOperationException("No se puede actualizar una entrada sin id.");

        var entity = await _context.WaitingLists
            .FirstOrDefaultAsync(x => x.Id == entry.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontró la entrada de lista de espera con id {entry.Id.Value}.");

        entity.Status = entry.Status;
    }

    private static WaitingLis MapToDomain(WaitingListEntity e)
        => WaitingLis.FromPrimitives(e.Id, e.BookingId, e.FlightId, e.RequestedAt, e.Priority, e.Status);

    private static WaitingListEntity MapToEntity(WaitingLis w)
        => new()
        {
            BookingId   = w.BookingId,
            FlightId    = w.FlightId,
            RequestedAt = w.RequestedAt,
            Priority    = w.Priority,
            Status      = w.Status
        };
}