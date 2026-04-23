using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Repositories;

public sealed class ContinentRepository : IContinentsRepository
{
    private readonly AppDbContext _context;

    public ContinentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Continent?> GetByIdAsync(ContinentsId id)
    {
        var entity = await _context.Continents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Continent>> GetAllAsync()
    {
        // Se ordena desde base para que cualquier menú o servicio reciba una lista consistente.
        var entities = await _context.Continents
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<Continent?> GetByNameAsync(string name)
    {
        var entity = await _context.Continents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name.Trim());

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(Continent continent)
    {
        var entity = MapToEntity(continent);
        await _context.Continents.AddAsync(entity);
    }

    public async Task UpdateAsync(Continent continent)
    {
        // Aquí se actualiza la entidad ya rastreada por EF para evitar problemas de tracking.
        var entity = await _context.Continents
            .FirstOrDefaultAsync(c => c.Id == continent.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Continent id '{continent.Id.Value}' not found.");

        entity.Name = continent.Name.Value;
    }

    public async Task DeleteAsync(ContinentsId id)
    {
        var entity = await _context.Continents
            .FirstOrDefaultAsync(c => c.Id == id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Continent with id '{id.Value}' not found.");

        _context.Continents.Remove(entity);
        
    }

    public async Task DeleteByNameAsync(string name)
    {
        var entity = await _context.Continents
            .FirstOrDefaultAsync(c => c.Name == name.Trim());

        if (entity is null)
            throw new InvalidOperationException($"Continent '{name}' not found.");

        _context.Continents.Remove(entity);
    }

    // Los mappers aíslan la forma de persistencia del modelo de dominio.

    private static Continent MapToDomain(ContinentEntity entity)
    {
        return Continent.Create(entity.Id, entity.Name);
    }

    private static ContinentEntity MapToEntity(Continent continent)
    {
        return new ContinentEntity
        {
            Name = continent.Name.Value
        };
    }
}
