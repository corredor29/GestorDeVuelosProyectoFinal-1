using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Repository;

public sealed class CardTypesRepository : ICardTypesRepository
{
    private readonly AppDbContext _context;

    public CardTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainAggregate?> GetByIdAsync(CardTypesId id)
    {
        var entity = await _context.Set<CardTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<DomainAggregate?> GetByNameAsync(CardTypesName name)
    {
        var entity = await _context.Set<CardTypesEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<DomainAggregate>> GetAllAsync()
    {
        var entities = await _context.Set<CardTypesEntity>().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(DomainAggregate cardTypes)
    {
        var entity = MapToEntity(cardTypes);
        await _context.Set<CardTypesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DomainAggregate cardTypes)
    {
        var entity = await _context.Set<CardTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == cardTypes.Id.Value);

        if (entity is null) return;

        entity.Name = cardTypes.Name.Value;

        _context.Set<CardTypesEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CardTypesId id)
    {
        var entity = await _context.Set<CardTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<CardTypesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static DomainAggregate MapToDomain(CardTypesEntity entity)
        => DomainAggregate.Create(entity.Id, entity.Name);

    private static CardTypesEntity MapToEntity(DomainAggregate domain)
        => new() { Name = domain.Name.Value };
}