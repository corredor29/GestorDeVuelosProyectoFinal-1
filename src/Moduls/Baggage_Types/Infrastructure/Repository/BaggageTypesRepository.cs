using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Repository;

public sealed class BaggageTypesRepository : IBaggageTypesRepository
{
    private readonly AppDbContext _context;

    public BaggageTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BaggageType?> GetByIdAsync(BaggageTypeId id)
    {
        var entity = await _context.Set<BaggageTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<BaggageType?> GetByNameAsync(BaggageTypeName name)
    {
        var entity = await _context.Set<BaggageTypesEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BaggageType>> GetAllAsync()
    {
        var entities = await _context.Set<BaggageTypesEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(BaggageType baggageType)
    {
        var entity = MapToEntity(baggageType);
        await _context.Set<BaggageTypesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        baggageType.SetId(entity.Id);
    }

    public async Task UpdateAsync(BaggageType baggageType)
    {
        var entity = await _context.Set<BaggageTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == baggageType.Id.Value);

        if (entity is null) return;

        entity.Name = baggageType.Name.Value;
        entity.MaxWeightKg = baggageType.MaxWeightKg.Value;
        entity.BasePrice = baggageType.BasePrice.Value;

        _context.Set<BaggageTypesEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BaggageTypeId id)
    {
        var entity = await _context.Set<BaggageTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<BaggageTypesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ── Mappers ──────────────────────────────────────────────────────────────

    private static BaggageType MapToDomain(BaggageTypesEntity entity)
    {
        return BaggageType.Create(entity.Id, entity.Name, entity.MaxWeightKg, entity.BasePrice);
    }

    private static BaggageTypesEntity MapToEntity(BaggageType domain)
    {
        return new BaggageTypesEntity
        {
            Name = domain.Name.Value,
            MaxWeightKg = domain.MaxWeightKg.Value,
            BasePrice = domain.BasePrice.Value
        };
    }
}