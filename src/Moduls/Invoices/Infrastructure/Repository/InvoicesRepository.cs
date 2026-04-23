using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Repository;

public sealed class InvoicesRepository : IInvoicesRepository
{
    private readonly AppDbContext _context;

    public InvoicesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(InvoicesId id)
    {
        var entity = await _context.Set<InvoicesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(InvoicesNumber numeroFactura)
    {
        var entity = await _context.Set<InvoicesEntity>()
            .FirstOrDefaultAsync(x => x.InvoiceNumber == numeroFactura.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> GetByReservationIdAsync(BookingId bookingId)
    {
        var entity = await _context.Set<InvoicesEntity>()
            .FirstOrDefaultAsync(x => x.Booking_Id == bookingId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(Invoice invoice)
    {
        var entity = MapToEntity(invoice);
        await _context.Set<InvoicesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        invoice.SetId(entity.Id);
    }

    public async Task DeleteAsync(InvoicesId id)
    {
        var entity = await _context.Set<InvoicesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<InvoicesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    private static Invoice MapToDomain(InvoicesEntity entity)
    {
        return Invoice.Create(
            entity.Id,
            entity.Booking_Id,
            entity.InvoiceNumber,
            entity.IssuedAt,
            entity.Subtotal,
            entity.Taxes,
            entity.Total
        );
    }

    private static InvoicesEntity MapToEntity(Invoice domain)
    {
        return new InvoicesEntity
        {
            Booking_Id = domain.ReservaId.Value,
            InvoiceNumber = domain.NumeroFactura.Value,
            IssuedAt = domain.FechaEmision.Value,
            Subtotal = domain.Subtotal.Value,
            Taxes = domain.Impuestos.Value,
            Total = domain.Total.Value,
            CreatedAt = DateTime.UtcNow
        };
    }
}