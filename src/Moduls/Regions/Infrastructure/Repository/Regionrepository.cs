using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Repositories;

public sealed class RegionRepository : IRegionsRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Region?> GetByIdAsync(RegionId id)
    {
        var entity = await _context.Regions
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Region?> GetByNameAsync(string name)
    {
        var entity = await _context.Regions
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name.Trim());

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Region>> GetByTypeAsync(string type)
    {
        var entities = await _context.Regions
            .AsNoTracking()
            .Where(r => r.Type == type.Trim())
            .OrderBy(r => r.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        var entities = await _context.Regions
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Region>> GetByCountryAsync(CountryId countryId)
    {
        // Este acceso soporta varios formularios donde primero se elige país y luego región.
        var entities = await _context.Regions
            .AsNoTracking()
            .Where(r => r.CountryId == countryId.Value)
            .OrderBy(r => r.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Region region)
    {
        var entity = MapToEntity(region);
        await _context.Regions.AddAsync(entity);

    }

    public async Task UpdateAsync(Region region)
    {
        var entity = await _context.Regions
            .FirstOrDefaultAsync(r => r.Id == region.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Region with id '{region.Id.Value}' not found.");

        entity.Name = region.Name.Value;
        entity.Type = region.Type.Value;
        entity.CountryId = region.CountryId.Value;

        _context.Regions.Update(entity);
    }

    public async Task DeleteAsync(RegionId id)
    {
        var entity = await _context.Regions
            .FirstOrDefaultAsync(r => r.Id == id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Region with id '{id.Value}' not found.");

        _context.Regions.Remove(entity);
      
    }

    public async Task DeleteByNameAsync(string name)
    {
        var entity = await _context.Regions
            .FirstOrDefaultAsync(r => r.Name == name.Trim());

        if (entity is null)
            throw new InvalidOperationException($"Region '{name}' not found.");

        _context.Regions.Remove(entity);
        
    }

    public async Task DeleteByTypeAsync(string type)
    {
        var entities = await _context.Regions
            .Where(r => r.Type == type.Trim())
            .ToListAsync();

        if (!entities.Any())
            throw new InvalidOperationException($"No regions of type '{type}' found.");

        _context.Regions.RemoveRange(entities);
        
    }

    // El mapeo sigue siendo directo porque la entidad de persistencia es bastante plana.

    private static Region MapToDomain(RegionEntity entity)
        => Region.Create(entity.Id, entity.Name, entity.Type, entity.CountryId);

    private static RegionEntity MapToEntity(Region region)
        => new RegionEntity
        {
            Name = region.Name.Value,
            Type = region.Type.Value,
            CountryId = region.CountryId.Value
        };
}
