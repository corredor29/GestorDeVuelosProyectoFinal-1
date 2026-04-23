using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Repository;

public sealed class InvoiceItemTypesRepository : IInvoiceItemTypesRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceItemType?> GetByIdAsync(InvoiceItemTypesId id)
    {
        var entity = await _context.Set<InvoiceItemTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<InvoiceItemType?> GetByNameAsync(InvoiceItemTypesName name)
    {
        var entity = await _context.Set<InvoiceItemTypesEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<InvoiceItemType>> GetAllAsync()
    {
        var entities = await _context.Set<InvoiceItemTypesEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(InvoiceItemType invoiceItemType)
    {
        var entity = MapToEntity(invoiceItemType);
        await _context.Set<InvoiceItemTypesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        invoiceItemType.SetId(entity.Id);
    }

    public async Task UpdateAsync(InvoiceItemType invoiceItemType)
    {
        var entity = await _context.Set<InvoiceItemTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == invoiceItemType.Id.Value);

        if (entity is null) return;

        entity.Name = invoiceItemType.Name.Value;

        _context.Set<InvoiceItemTypesEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(InvoiceItemTypesId id)
    {
        var entity = await _context.Set<InvoiceItemTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<InvoiceItemTypesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ── Mappers ──────────────────────────────────────────────────────────────

    private static InvoiceItemType MapToDomain(InvoiceItemTypesEntity entity)
    {
        return InvoiceItemType.Create(entity.Id, entity.Name);
    }

    private static InvoiceItemTypesEntity MapToEntity(InvoiceItemType domain)
    {
        return new InvoiceItemTypesEntity
        {
            Name = domain.Name.Value
        };
    }
}