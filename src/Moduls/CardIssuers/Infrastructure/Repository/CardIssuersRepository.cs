using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Repository;

public sealed class CardIssuersRepository : ICardIssuersRepository
{
    private readonly AppDbContext _context;

    public CardIssuersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainAggregate?> GetByIdAsync(CardIssuersId id)
    {
        var entity = await _context.Set<CardIssuerEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<DomainAggregate?> GetByNameAsync(CardIssuersName name)
    {
        var entity = await _context.Set<CardIssuerEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<DomainAggregate>> GetAllAsync()
    {
        var entities = await _context.Set<CardIssuerEntity>().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(DomainAggregate cardIssuer)
    {
        var entity = MapToEntity(cardIssuer);
        await _context.Set<CardIssuerEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DomainAggregate cardIssuer)
    {
        var entity = await _context.Set<CardIssuerEntity>()
            .FirstOrDefaultAsync(x => x.Id == cardIssuer.Id.Value);

        if (entity is null) return;

        entity.Name         = cardIssuer.Name.Value;
        entity.IssuerNumber = cardIssuer.IssuerNumber.Value;

        _context.Set<CardIssuerEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CardIssuersId id)
    {
        var entity = await _context.Set<CardIssuerEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<CardIssuerEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static DomainAggregate MapToDomain(CardIssuerEntity entity)
        => DomainAggregate.Create(entity.Id, entity.Name, entity.IssuerNumber);

    private static CardIssuerEntity MapToEntity(DomainAggregate domain)
        => new()
        {
            Name         = domain.Name.Value,
            IssuerNumber = domain.IssuerNumber.Value
        };
}