using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Repository;

public sealed class BaggageRepository : IBaggageRepository
{
    private readonly AppDbContext _context;

    public BaggageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BaggageRoot?> FindByIdAsync(BaggageId id)
    {
        var entity = await _context.Set<BaggageEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BaggageRoot>> FindAllAsync()
    {
        var entities = await _context.Set<BaggageEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<BaggageRoot>> FindByCheckinIdAsync(int checkinId)
    {
        var entities = await _context.Set<BaggageEntity>()
            .Where(x => x.CheckinId == checkinId)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(BaggageRoot baggage)
    {
        var entity = MapToEntity(baggage);
        await _context.Set<BaggageEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BaggageRoot baggage)
    {
        var entity = await _context.Set<BaggageEntity>()
            .FirstOrDefaultAsync(x => x.Id == baggage.Id.Value);

        if (entity is null) return;

        entity.WeightKg      = baggage.WeightKg.Value;
        entity.ChargedPrice  = baggage.ChargedPrice.Value;

        _context.Set<BaggageEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BaggageId id)
    {
        var entity = await _context.Set<BaggageEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<BaggageEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static BaggageRoot MapToDomain(BaggageEntity entity)
    {
        return BaggageRoot.Create(
            entity.Id,
            entity.CheckinId,
            entity.BaggageTypeId,
            entity.WeightKg,
            entity.ChargedPrice);
    }

    private static BaggageEntity MapToEntity(BaggageRoot domain)
    {
        return new BaggageEntity
        {
            CheckinId     = domain.CheckinId.Value,
            BaggageTypeId = domain.BaggageTypeId.Value,
            WeightKg      = domain.WeightKg.Value,
            ChargedPrice  = domain.ChargedPrice.Value
        };
    }
}