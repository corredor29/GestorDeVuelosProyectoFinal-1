using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Repository;

public sealed class BookingStatusTransitionsRepository : IBookingStatusTransitionsRepository, IBookingStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public BookingStatusTransitionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookingStatusTransition?> GetByIdAsync(BookingStatusTransitionsId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BookingStatusTransitions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BookingStatusTransition>> GetByFromStatusIdAsync(BookingStatusesId fromStatusId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.BookingStatusTransitions.AsNoTracking()
            .Where(x => x.FromStatusId == fromStatusId.Value)
            .OrderBy(x => x.ToStatusId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<BookingStatusTransition?> GetByStatusesAsync(BookingStatusesId fromStatusId, BookingStatusesId toStatusId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BookingStatusTransitions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.FromStatusId == fromStatusId.Value && x.ToStatusId == toStatusId.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BookingStatusTransition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.BookingStatusTransitions.AsNoTracking()
            .OrderBy(x => x.FromStatusId)
            .ThenBy(x => x.ToStatusId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(BookingStatusTransition transition, CancellationToken cancellationToken = default)
    {
        await _context.BookingStatusTransitions.AddAsync(new BookingStatusTransitionEntity
        {
            FromStatusId = transition.FromStatusId.Value,
            ToStatusId = transition.ToStatusId.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(BookingStatusTransition transition, CancellationToken cancellationToken = default)
    {
        if (transition.Id is null)
            throw new InvalidOperationException("No se puede actualizar una transicion sin id.");

        var entity = await _context.BookingStatusTransitions
            .FirstOrDefaultAsync(x => x.Id == transition.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro la transicion con id {transition.Id.Value}.");

        entity.FromStatusId = transition.FromStatusId.Value;
        entity.ToStatusId = transition.ToStatusId.Value;
    }

    public async Task DeleteByIdAsync(BookingStatusTransitionsId id, CancellationToken cancellationToken = default)
    {
        await _context.BookingStatusTransitions
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> ValidateTransitionAsync(BookingStatusesId fromStatusId, BookingStatusesId toStatusId, CancellationToken cancellationToken = default)
    {
        return _context.BookingStatusTransitions.AsNoTracking()
            .AnyAsync(x => x.FromStatusId == fromStatusId.Value && x.ToStatusId == toStatusId.Value, cancellationToken);
    }

    public Task<bool> IsTransitionAllowedAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
    {
        return _context.BookingStatusTransitions.AsNoTracking()
            .AnyAsync(x => x.FromStatusId == fromStatusId && x.ToStatusId == toStatusId, cancellationToken);
    }

    public async Task<IReadOnlyList<int>> GetAllowedToStatusIdsAsync(int fromStatusId, CancellationToken cancellationToken = default)
    {
        return await _context.BookingStatusTransitions.AsNoTracking()
            .Where(x => x.FromStatusId == fromStatusId)
            .OrderBy(x => x.ToStatusId)
            .Select(x => x.ToStatusId)
            .ToListAsync(cancellationToken);
    }

    private static BookingStatusTransition MapToDomain(BookingStatusTransitionEntity entity)
        => BookingStatusTransition.FromPrimitives(entity.Id, entity.FromStatusId, entity.ToStatusId);
}
