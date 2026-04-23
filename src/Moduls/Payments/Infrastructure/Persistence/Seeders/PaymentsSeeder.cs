using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Persistence.Seeders;

public static class PaymentsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Payments.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var bookingIds = await db.Bookings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (bookingIds.Count == 0)
            return;

        var paidStatusId = await db.PaymentStatuses
            .AsNoTracking()
            .Where(x => x.Name == "Pagado")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var paymentMethodId = await db.PaymentMethods
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (paidStatusId is null || paymentMethodId is null)
            return;

        var bookings = await db.Bookings
            .AsNoTracking()
            .Where(x => bookingIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        foreach (var b in bookings)
        {
            await db.Payments.AddAsync(new PaymentsEntity
            {
                BookingId = b.Id,
                Amount = b.TotalAmount,
                PaidAt = now,
                PaymentStatusId = paidStatusId.Value,
                PaymentMethodId = paymentMethodId.Value,
                CreatedAt = now,
                UpdatedAt = now
            }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

