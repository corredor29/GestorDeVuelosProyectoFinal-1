using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Repository;

public sealed class AirportsRepository : IAirportsRepository
{
    private readonly AppDbContext _context;

    public AirportsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Airport?> GetByIdAsync(AirportsId id)
    {
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Airport?> GetByNameAsync(AirportsName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Airport?> GetByIataCodeAsync(AirportsIataCode iataCode)
    {
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IataCode == iataCode.Value);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Airport?> GetByIcaoCodeAsync(AirportsIcaoCode icaoCode)
    {
        var normalized = icaoCode.Value;
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IcaoCode == normalized);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Airport>> GetByCityIdAsync(CityId cityId)
    {
        var entities = await _context.Airports
            .AsNoTracking()
            .Where(x => x.CityId == cityId.Value)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Airport>> GetByCityNameAsync(string cityName)
    {
        // Este join evita que la UI tenga que resolver primero la ciudad solo para filtrar aeropuertos.
        var normalized = cityName.Trim().ToLower();

        var entities = await (
            from airport in _context.Airports.AsNoTracking()
            join city in _context.Cities.AsNoTracking() on airport.CityId equals city.Id
            where city.Name.ToLower().Contains(normalized)
            orderby airport.Name
            select airport
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Airport>> GetAllAsync()
    {
        var entities = await _context.Airports
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Airport airport)
    {
        await _context.Airports.AddAsync(MapToEntity(airport));
    }

    public Task UpdateAsync(Airport airport)
    {
        // Se reenvía la entidad armada desde el agregado y EF resuelve la escritura.
        _context.Airports.Update(MapToEntity(airport));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(AirportsId id)
    {
        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        await EnsureNoActiveRoutesAsync(entity.Id);
        _context.Airports.Remove(entity);
    }

    public async Task DeleteByNameAsync(AirportsName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        if (entity is null)
            return;

        await EnsureNoActiveRoutesAsync(entity.Id);
        _context.Airports.Remove(entity);
    }

    public async Task DeleteByIataCodeAsync(AirportsIataCode iataCode)
    {
        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.IataCode == iataCode.Value);
        if (entity is null)
            return;

        await EnsureNoActiveRoutesAsync(entity.Id);
        _context.Airports.Remove(entity);
    }

    public async Task DeleteByIcaoCodeAsync(AirportsIcaoCode icaoCode)
    {
        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.IcaoCode == icaoCode.Value);
        if (entity is null)
            return;

        await EnsureNoActiveRoutesAsync(entity.Id);
        _context.Airports.Remove(entity);
    }

    private async Task EnsureNoActiveRoutesAsync(int airportId)
    {
        // No se deja borrar un aeropuerto si todavía participa en rutas para no romper integridad.
        var hasRoutes = await _context.Routes
            .AsNoTracking()
            .AnyAsync(x => x.OriginAirportId == airportId || x.DestinationAirportId == airportId);

        if (hasRoutes)
            throw new InvalidOperationException("No se puede eliminar el aeropuerto porque tiene rutas activas asociadas.");
    }

    private static Airport MapToDomain(AirportEntity entity)
    {
        return Airport.FromPrimitives(entity.Id, entity.Name, entity.IataCode, entity.IcaoCode, entity.CityId);
    }

    private static AirportEntity MapToEntity(Airport aggregate)
    {
        return new AirportEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            Name = aggregate.Name.Value,
            IataCode = aggregate.IataCode.Value,
            IcaoCode = aggregate.IcaoCode.Value,
            CityId = aggregate.CityId.Value
        };
    }
}
