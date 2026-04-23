using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Repository;

public sealed class RouteStopoversRepository : IRouteStopoversRepository
{
    private readonly AppDbContext _context;

    public RouteStopoversRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RouteStopover?> GetByIdAsync(RouteStopOversId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.RouteStopovers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<RouteStopover>> GetByRouteIdAsync(RouteId routeId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.RouteStopovers.AsNoTracking()
            .Where(x => x.RouteId == routeId.Value)
            .OrderBy(x => x.StopOrder)
            .ToListAsync(cancellationToken);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<RouteStopover?> GetByRouteIdAndStopOrderAsync(
        RouteId routeId,
        RouteStopOrder stopOrder,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.RouteStopovers.AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.RouteId == routeId.Value && x.StopOrder == stopOrder.Value,
                cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<RouteStopover>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.RouteStopovers.AsNoTracking()
            .OrderBy(x => x.RouteId).ThenBy(x => x.StopOrder)
            .ToListAsync(cancellationToken);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(RouteStopover routeStopover, CancellationToken cancellationToken = default)
    {
        await _context.RouteStopovers.AddAsync(new RouteStopoversEntity
        {
            RouteId = routeStopover.RouteId.Value,
            StopoverAirportId = routeStopover.StopoverAirportId.Value,
            StopOrder = routeStopover.StopOrder.Value,
            LayoverMin = routeStopover.LayoverMin.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(RouteStopover routeStopover, CancellationToken cancellationToken = default)
    {
        if (routeStopover.Id is null)
            throw new InvalidOperationException("No se puede actualizar una escala sin id.");

        var entity = await _context.RouteStopovers
            .FirstOrDefaultAsync(x => x.Id == routeStopover.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontró la escala con id '{routeStopover.Id.Value}'.");

        entity.RouteId = routeStopover.RouteId.Value;
        entity.StopoverAirportId = routeStopover.StopoverAirportId.Value;
        entity.StopOrder = routeStopover.StopOrder.Value;
        entity.LayoverMin = routeStopover.LayoverMin.Value;
    }

    public async Task DeleteAsync(RouteStopOversId id, CancellationToken cancellationToken = default)
    {
        await _context.RouteStopovers
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<int> DeleteByRouteIdAsync(RouteId routeId, CancellationToken cancellationToken = default)
        => _context.RouteStopovers
            .Where(x => x.RouteId == routeId.Value)
            .ExecuteDeleteAsync(cancellationToken);

    private static RouteStopover MapToDomain(RouteStopoversEntity e)
        => RouteStopover.FromPrimitives(e.Id, e.RouteId, e.StopoverAirportId, e.StopOrder, e.LayoverMin);
}
