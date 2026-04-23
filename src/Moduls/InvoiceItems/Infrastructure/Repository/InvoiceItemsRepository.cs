using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Repository;

public sealed class InvoiceItemsRepository : IInvoiceItemsRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceItem?> GetByIdAsync(InvoiceItemsId id)
    {
        var entity = await _context.Set<InvoiceItemsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<InvoiceItem>> GetByFacturaIdAsync(InvoicesId facturaId)
    {
        var entities = await _context.Set<InvoiceItemsEntity>()
            .Where(x => x.Invoice_Id == facturaId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(InvoiceItem item)
    {
        var entity = MapToEntity(item);
        await _context.Set<InvoiceItemsEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        item.SetId(entity.Id);
    }

    public async Task DeleteAsync(InvoiceItemsId id)
    {
        var entity = await _context.Set<InvoiceItemsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<InvoiceItemsEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ── Mappers ──────────────────────────────────────────────────────────────

    private static InvoiceItem MapToDomain(InvoiceItemsEntity entity)
    {
        return InvoiceItem.Create(
            entity.Id,
            entity.Invoice_Id,
            entity.Item_Type_Id,
            entity.Description,
            entity.Quantity,
            entity.UnitPrice,
            entity.Subtotal,
            entity.BookingPassenger_Id
        );
    }

    private static InvoiceItemsEntity MapToEntity(InvoiceItem domain)
    {
        return new InvoiceItemsEntity
        {
            Invoice_Id = domain.FacturaId.Value,
            Item_Type_Id = domain.TipoItemId.Value,
            Description = domain.Descripcion.Value,
            Quantity = domain.Cantidad.Value,
            UnitPrice = domain.PrecioUnitario.Value,
            Subtotal = domain.Subtotal.Value,
            BookingPassenger_Id = domain.ReservaPasajeroId
        };
    }
}