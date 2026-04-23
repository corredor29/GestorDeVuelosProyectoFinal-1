using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Repository;

// Repositorio del catálogo de tipos de vía.
// Se usa sobre todo desde direcciones para no escribir el tipo de vía libremente.
public sealed class StreetTypeRepository : IStreetTypesRepository
{
    private readonly AppDbContext _context;

    public StreetTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StreetType?> GetByIdAsync(StreetTypeId id)
    {
        var entity = await _context.StreetTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<StreetType?> GetByNameAsync(string name)
    {
        // Igualamos por texto normalizado para que la búsqueda no dependa de mayúsculas.
        var entity = await _context.StreetTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.Trim().ToLower());

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<StreetType>> GetAllAsync()
    {
        var entities = await _context.StreetTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(StreetType streetType)
    {
        var entity = MapToEntity(streetType);
        await _context.StreetTypes.AddAsync(entity);
    }

    public async Task SaveAsync(StreetType streetType)
    {
        // Aquí SaveAsync realmente actúa como actualización del nombre.
        var entity = await _context.StreetTypes
            .FirstOrDefaultAsync(x => x.Id == streetType.Id.Value)
            ?? throw new InvalidOperationException($"No existe tipo de vía con id {streetType.Id.Value}.");

        entity.Name = streetType.Name.Value;
    }

    public async Task DeleteAsync(StreetTypeId id)
    {
        var entity = await _context.StreetTypes.FindAsync(id.Value);
        if (entity != null)
        {
            _context.StreetTypes.Remove(entity);
        }
    }

    public async Task DeleteByNameAsync(string name)
    {
        var entity = await _context.StreetTypes
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.Trim().ToLower());
        
        if (entity != null)
        {
            _context.StreetTypes.Remove(entity);
        }
    }

    // --- Mappers Privados ---

    private static StreetType MapToDomain(StreetTypeEntity entity)
    {
        return StreetType.Create(entity.Id, entity.Name);
    }

    private static StreetTypeEntity MapToEntity(StreetType aggregate)
    {
        return new StreetTypeEntity
        {
            Id = aggregate.Id.Value,
            Name = aggregate.Name.Value
        };
    }
}
