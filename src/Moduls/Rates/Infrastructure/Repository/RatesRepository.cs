using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Repository;

public sealed class RatesRepository : IRatesRepository
{
    private readonly AppDbContext _context;

    public RatesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rate?> GetByIdAsync(RatesId id)
    {
        var entity = await _context.Rates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Rate>> GetByCombinationAsync(RouteId routeId, CabinTypesId cabinTypeId, PassengerTypesId passengerTypeId, SeasonsId seasonId)
    {
        var entities = await _context.Rates
            .AsNoTracking()
            .Where(x => x.RouteId == routeId.Value
                && x.CabinTypeId == cabinTypeId.Value
                && x.PassengerTypeId == passengerTypeId.Value
                && x.SeasonId == seasonId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Rate>> GetByRouteIdAsync(RouteId routeId)
    {
        var entities = await _context.Rates
            .AsNoTracking()
            .Where(x => x.RouteId == routeId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Rate>> GetByCabinTypeIdAsync(CabinTypesId cabinTypeId)
    {
        var entities = await _context.Rates
            .AsNoTracking()
            .Where(x => x.CabinTypeId == cabinTypeId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Rate>> GetAllAsync()
    {
        var entities = await _context.Rates
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Rate rate)
    {
        await _context.Rates.AddAsync(MapToEntity(rate));
    }

    public Task UpdateAsync(Rate rate)
    {
        _context.Rates.Update(MapToEntity(rate));
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(RatesId id)
    {
        var entity = await _context.Rates.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            _context.Rates.Remove(entity);
    }

    public Task<bool> ExistsBySeasonIdAsync(SeasonsId seasonId)
    {
        return _context.Rates.AsNoTracking().AnyAsync(x => x.SeasonId == seasonId.Value);
    }

    private static Rate MapToDomain(RateEntity entity)
    {
        return Rate.FromPrimitives(
            entity.Id,
            entity.RouteId,
            entity.CabinTypeId,
            entity.PassengerTypeId,
            entity.SeasonId,
            entity.BasePrice,
            entity.ValidFrom,
            entity.ValidUntil);
    }

    private static RateEntity MapToEntity(Rate aggregate)
    {
        return new RateEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            RouteId = aggregate.RouteId.Value,
            CabinTypeId = aggregate.CabinTypeId.Value,
            PassengerTypeId = aggregate.PassengerTypeId.Value,
            SeasonId = aggregate.SeasonId.Value,
            BasePrice = aggregate.BasePrice.Value,
            ValidFrom = aggregate.ValidFrom.Value,
            ValidUntil = aggregate.ValidUntil.Value
        };
    }
}
