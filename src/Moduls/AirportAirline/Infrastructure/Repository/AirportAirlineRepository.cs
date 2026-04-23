using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Repository;

public sealed class AirportAirlineRepository : IAirportAirlineRepository
{
    private readonly AppDbContext _context;

    public AirportAirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AirportAirlineOperation?> GetByIdAsync(AirportAirlineId id)
    {
        var entity = await _context.AirportAirlines.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetAllAsync()
    {
        // Se ordena por aeropuerto y aerolínea para que el listado tenga un patrón estable.
        var entities = await _context.AirportAirlines.AsNoTracking().OrderBy(x => x.AirportId).ThenBy(x => x.AirlineId).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetActiveAsync()
    {
        var entities = await _context.AirportAirlines.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.AirportId).ThenBy(x => x.AirlineId).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetByTerminalAsync(AirportAirlineTerminal terminal)
    {
        var normalized = terminal.Value?.Trim().ToLower() ?? string.Empty;
        var entities = await _context.AirportAirlines.AsNoTracking()
            .Where(x => (x.Terminal ?? string.Empty).ToLower() == normalized)
            .ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetByAirportIdAsync(AirportsId airportId)
    {
        var entities = await _context.AirportAirlines.AsNoTracking().Where(x => x.AirportId == airportId.Value).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetByAirlineIdAsync(AirlinesId airlineId)
    {
        var entities = await _context.AirportAirlines.AsNoTracking().Where(x => x.AirlineId == airlineId.Value).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetByStartDateAsync(AirportAirlineStartDate startDate)
    {
        var date = startDate.Value.Date;
        var entities = await _context.AirportAirlines.AsNoTracking().Where(x => x.StartDate.Date == date).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AirportAirlineOperation>> GetByEndDateAsync(AirportAirlineEndDate endDate)
    {
        if (!endDate.Value.HasValue)
            return Enumerable.Empty<AirportAirlineOperation>();

        var date = endDate.Value.Value.Date;
        var entities = await _context.AirportAirlines.AsNoTracking().Where(x => x.EndDate.HasValue && x.EndDate.Value.Date == date).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<AirportAirlineOperation?> GetByAirportAndAirlineAsync(AirportsId airportId, AirlinesId airlineId)
    {
        var entity = await _context.AirportAirlines.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AirportId == airportId.Value && x.AirlineId == airlineId.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(AirportAirlineOperation airportAirline)
    {
        await _context.AirportAirlines.AddAsync(MapToEntity(airportAirline));
    }

    public Task UpdateAsync(AirportAirlineOperation airportAirline)
    {
        // EF recibe la entidad completa porque este agregado tiene varios campos coordinados.
        _context.AirportAirlines.Update(MapToEntity(airportAirline));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(AirportAirlineId id)
    {
        var entity = await _context.AirportAirlines.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            entity.IsActive = false;
    }

    public async Task<int> DeleteByTerminalAsync(AirportAirlineTerminal terminal)
    {
        var normalized = terminal.Value?.Trim().ToLower() ?? string.Empty;
        var entities = await _context.AirportAirlines.Where(x => (x.Terminal ?? string.Empty).ToLower() == normalized && x.IsActive).ToListAsync();
        foreach (var entity in entities) entity.IsActive = false;
        return entities.Count;
    }

    public async Task<int> DeleteByAirportIdAsync(AirportsId airportId)
    {
        var entities = await _context.AirportAirlines.Where(x => x.AirportId == airportId.Value && x.IsActive).ToListAsync();
        foreach (var entity in entities) entity.IsActive = false;
        return entities.Count;
    }

    public async Task<int> DeleteByAirlineIdAsync(AirlinesId airlineId)
    {
        // La desactivación masiva sirve cuando una aerolínea deja de operar en varios aeropuertos.
        var entities = await _context.AirportAirlines.Where(x => x.AirlineId == airlineId.Value && x.IsActive).ToListAsync();
        foreach (var entity in entities) entity.IsActive = false;
        return entities.Count;
    }

    public async Task<int> DeleteByStartDateAsync(AirportAirlineStartDate startDate)
    {
        var date = startDate.Value.Date;
        var entities = await _context.AirportAirlines.Where(x => x.StartDate.Date == date && x.IsActive).ToListAsync();
        foreach (var entity in entities) entity.IsActive = false;
        return entities.Count;
    }

    public async Task<int> DeleteByEndDateAsync(AirportAirlineEndDate endDate)
    {
        if (!endDate.Value.HasValue)
            return 0;

        var date = endDate.Value.Value.Date;
        var entities = await _context.AirportAirlines.Where(x => x.EndDate.HasValue && x.EndDate.Value.Date == date && x.IsActive).ToListAsync();
        foreach (var entity in entities) entity.IsActive = false;
        return entities.Count;
    }

    public async Task ReactivateAsync(AirportAirlineId id)
    {
        var entity = await _context.AirportAirlines.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            entity.IsActive = true;
    }

    private static AirportAirlineOperation MapToDomain(AirportAirlineEntity entity)
    {
        return AirportAirlineOperation.FromPrimitives(entity.Id, entity.AirportId, entity.AirlineId, entity.Terminal, entity.StartDate, entity.EndDate, entity.IsActive);
    }

    private static AirportAirlineEntity MapToEntity(AirportAirlineOperation aggregate)
    {
        return new AirportAirlineEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            AirportId = aggregate.AirportId.Value,
            AirlineId = aggregate.AirlineId.Value,
            Terminal = aggregate.Terminal.Value,
            StartDate = aggregate.StartDate.Value,
            EndDate = aggregate.EndDate.Value,
            IsActive = aggregate.IsActive.Value
        };
    }
}
