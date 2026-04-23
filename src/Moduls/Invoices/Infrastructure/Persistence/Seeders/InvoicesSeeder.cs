using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Persistence.Seeders;

public static class InvoicesSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        // Si ya hay facturas, no duplicamos (hay índice único por booking_id)
        if (await db.Invoices.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var bookings = await db.Bookings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (bookings.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var i = 1;
        foreach (var b in bookings)
        {
            await db.Invoices.AddAsync(new InvoicesEntity
            {
                Booking_Id = b.Id,
                InvoiceNumber = $"INV-{i:0000}",
                IssuedAt = now,
                Subtotal = b.TotalAmount,
                Taxes = 0m,
                Total = b.TotalAmount,
                CreatedAt = now
            }, cancellationToken);
            i++;
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

