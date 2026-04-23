using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Repository;

public sealed class RoutesRepository : IRoutesRepository
{
    private readonly AppDbContext _context;

    public RoutesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Route?> GetByIdAsync(RouteId id)
    {
        var entity = await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Route>> GetByOriginAirportIdAsync(AirportsId originAirportId)
    {
        var entities = await _context.Routes
            .AsNoTracking()
            .Where(x => x.OriginAirportId == originAirportId.Value)
            .OrderBy(x => x.DestinationAirportId)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Route>> GetByDestinationAirportIdAsync(AirportsId destinationAirportId)
    {
        var entities = await _context.Routes
            .AsNoTracking()
            .Where(x => x.DestinationAirportId == destinationAirportId.Value)
            .OrderBy(x => x.OriginAirportId)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<Route?> GetByOriginAndDestinationAsync(AirportsId origin, AirportsId destination)
    {
        var entity = await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginAirportId == origin.Value && x.DestinationAirportId == destination.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Route>> GetAllAsync()
    {
        // El orden por origen/destino hace que el listado sea más fácil de leer y comparar.
        var entities = await _context.Routes
            .AsNoTracking()
            .OrderBy(x => x.OriginAirportId)
            .ThenBy(x => x.DestinationAirportId)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Route route)
    {
        await _context.Routes.AddAsync(MapToEntity(route));
    }

    public Task UpdateAsync(Route route)
    {
        // El agregado ya llega validado, así que aquí solo se persiste el nuevo estado.
        _context.Routes.Update(MapToEntity(route));
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(RouteId id)
    {
        var entity = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            _context.Routes.Remove(entity);
    }

    private static Route MapToDomain(RouteEntity entity)
    {
        return Route.FromPrimitives(
            entity.Id,
            entity.OriginAirportId,
            entity.DestinationAirportId,
            entity.DistanceKm,
            entity.EstimatedDurationMin);
    }

    private static RouteEntity MapToEntity(Route route)
    {
        return new RouteEntity
        {
            Id = route.Id?.Value ?? 0,
            OriginAirportId = route.OriginAirportId.Value,
            DestinationAirportId = route.DestinationAirportId.Value,
            DistanceKm = route.Distance.Value,
            EstimatedDurationMin = route.Duration.Value
        };
    }
}
