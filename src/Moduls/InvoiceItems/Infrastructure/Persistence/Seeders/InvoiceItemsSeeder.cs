using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Persistence.Seeders;

public static class InvoiceItemsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.InvoiceItems.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var invoices = await db.Invoices
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (invoices.Count == 0)
            return;

        var serviceTypeId = await db.InvoiceItemTypes
            .AsNoTracking()
            .Where(x => x.Name == "Servicio")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (serviceTypeId is null)
            return;

        foreach (var inv in invoices)
        {
            await db.InvoiceItems.AddAsync(new InvoiceItemsEntity
            {
                Invoice_Id = inv.Id,
                Item_Type_Id = serviceTypeId.Value,
                Description = $"Servicio asociado a la reserva #{inv.Booking_Id}",
                Quantity = 1,
                UnitPrice = inv.Subtotal,
                Subtotal = inv.Subtotal,
                BookingPassenger_Id = null
            }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

