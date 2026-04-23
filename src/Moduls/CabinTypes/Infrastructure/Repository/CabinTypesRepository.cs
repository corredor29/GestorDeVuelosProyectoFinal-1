using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Repository;

public sealed class CabinTypesRepository : ICabinTypesRepository
{
    private readonly AppDbContext _context;

    public CabinTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CabinType?> GetByIdAsync(CabinTypesId id)
    {
        var entity = await _context.CabinTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity is null ? null : CabinType.FromPrimitives(entity.Id, entity.Name);
    }

    public async Task<CabinType?> GetByNameAsync(CabinTypesName name)
    {
        var normalized = name.Value.Trim();
        var entity = await _context.CabinTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower());
        return entity is null ? null : CabinType.FromPrimitives(entity.Id, entity.Name);
    }

    public Task<CabinType?> GetByNameStringAsync(string name)
        => GetByNameAsync(CabinTypesName.Create(name));

    public async Task<IEnumerable<CabinType>> GetAllAsync()
    {
        var entities = await _context.CabinTypes.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        return entities.Select(x => CabinType.FromPrimitives(x.Id, x.Name));
    }

    public async Task SaveAsync(CabinType cabinType)
    {
        await _context.CabinTypes.AddAsync(new CabinTypeEntity
        {
            Name = cabinType.Name.Value
        });
    }

    public async Task UpdateAsync(CabinType cabinType)
    {
        var entity = await _context.CabinTypes.FirstOrDefaultAsync(x => x.Id == cabinType.Id.Value);
        if (entity is null)
            throw new InvalidOperationException($"Cabin type with id '{cabinType.Id.Value}' not found.");

        entity.Name = cabinType.Name.Value;
    }

    public async Task DeleteAsync(CabinTypesId id)
    {
        var entity = await _context.CabinTypes.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            _context.CabinTypes.Remove(entity);
    }
}
