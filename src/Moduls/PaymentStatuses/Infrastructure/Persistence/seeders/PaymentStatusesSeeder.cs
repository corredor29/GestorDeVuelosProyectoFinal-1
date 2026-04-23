using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Persistence.seeders;

public static class PaymentStatusesSeeder
{
    private static readonly string[] Statuses =
    [
        "Pendiente",
        "Pagado",
        "Fallido",
        "Reembolsado"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.PaymentStatuses
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in Statuses)
        {
            if (existing.Contains(name))
                continue;

            await db.PaymentStatuses.AddAsync(new PaymentStatusesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

