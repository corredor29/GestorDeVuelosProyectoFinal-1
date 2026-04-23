using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Repository;

public sealed class SeasonsRepository : ISeasonsRepository
{
    private readonly AppDbContext _context;

    public SeasonsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Season?> GetByIdAsync(SeasonsId id)
    {
        var entity = await _context.Seasons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Season?> GetByNameAsync(SeasonsName name)
    {
        var normalized = name.Value.Trim().ToLower();

        var entity = await _context.Seasons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Season>> GetAllAsync()
    {
        var entities = await _context.Seasons
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Season season)
    {
        await _context.Seasons.AddAsync(MapToEntity(season));
    }

    public Task UpdateAsync(Season season)
    {
        return UpdateEntityAsync(season);
    }

    public async Task DeleteByIdAsync(SeasonsId id)
    {
        var entity = await _context.Seasons.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        await EnsureSeasonHasNoRatesAsync(entity.Id);
        _context.Seasons.Remove(entity);
    }

    public async Task DeleteByNameAsync(SeasonsName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.Seasons.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        if (entity is null)
            return;

        await EnsureSeasonHasNoRatesAsync(entity.Id);
        _context.Seasons.Remove(entity);
    }

    private static Season MapToDomain(SeasonEntity entity)
    {
        return Season.FromPrimitives(entity.Id, entity.Name, entity.Description, entity.PriceFactor);
    }

    private static SeasonEntity MapToEntity(Season aggregate)
    {
        return new SeasonEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            Name = aggregate.Name.Value,
            Description = aggregate.Description.Value,
            PriceFactor = aggregate.PriceFactor.Value
        };
    }

    private async Task UpdateEntityAsync(Season aggregate)
    {
        var id = aggregate.Id?.Value ?? 0;
        if (id <= 0)
            throw new InvalidOperationException("No se puede actualizar una temporada sin ID.");

        var tracked = _context.Seasons.Local.FirstOrDefault(x => x.Id == id);
        var entity = tracked ?? await _context.Seasons.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"No se encontró la temporada con id '{id}'.");

        entity.Name = aggregate.Name.Value;
        entity.Description = aggregate.Description.Value;
        entity.PriceFactor = aggregate.PriceFactor.Value;
    }

    private async Task EnsureSeasonHasNoRatesAsync(int seasonId)
    {
        var hasRates = await _context.Rates
            .AsNoTracking()
            .AnyAsync(x => x.SeasonId == seasonId);

        if (hasRates)
            throw new InvalidOperationException("No se puede eliminar la temporada porque tiene tarifas asociadas.");
    }
}
