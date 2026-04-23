using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Repository;

public sealed class FlightStatusTransitionsRepository : IFlightStatusTransitionsRepository
{
    private readonly AppDbContext _context;

    public FlightStatusTransitionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FlightStatusTransition?> GetByIdAsync(FlightStatusTransitionsId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightStatusTransitions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightStatusTransition>> GetByFromStatusIdAsync(FlightStatusId fromStatusId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightStatusTransitions.AsNoTracking()
            .Where(x => x.FromStatusId == fromStatusId.Value)
            .OrderBy(x => x.ToStatusId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<FlightStatusTransition?> GetByStatusesAsync(FlightStatusId fromStatusId, FlightStatusId toStatusId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightStatusTransitions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.FromStatusId == fromStatusId.Value && x.ToStatusId == toStatusId.Value, cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightStatusTransition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightStatusTransitions.AsNoTracking()
            .OrderBy(x => x.FromStatusId)
            .ThenBy(x => x.ToStatusId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(FlightStatusTransition transition, CancellationToken cancellationToken = default)
    {
        await _context.FlightStatusTransitions.AddAsync(new FlightStatusTransitionEntity
        {
            FromStatusId = transition.FromStatusId.Value,
            ToStatusId = transition.ToStatusId.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(FlightStatusTransition transition, CancellationToken cancellationToken = default)
    {
        if (transition.Id is null)
            throw new InvalidOperationException("No se puede actualizar una transicion sin id.");

        var entity = await _context.FlightStatusTransitions
            .FirstOrDefaultAsync(x => x.Id == transition.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro la transicion con id {transition.Id.Value}.");

        entity.FromStatusId = transition.FromStatusId.Value;
        entity.ToStatusId = transition.ToStatusId.Value;
    }

    public async Task DeleteByIdAsync(FlightStatusTransitionsId id, CancellationToken cancellationToken = default)
    {
        await _context.FlightStatusTransitions
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> ValidateTransitionAsync(FlightStatusId fromStatusId, FlightStatusId toStatusId, CancellationToken cancellationToken = default)
    {
        return _context.FlightStatusTransitions.AsNoTracking()
            .AnyAsync(x => x.FromStatusId == fromStatusId.Value && x.ToStatusId == toStatusId.Value, cancellationToken);
    }

    private static FlightStatusTransition MapToDomain(FlightStatusTransitionEntity e)
        => FlightStatusTransition.FromPrimitives(e.Id, e.FromStatusId, e.ToStatusId);
}
