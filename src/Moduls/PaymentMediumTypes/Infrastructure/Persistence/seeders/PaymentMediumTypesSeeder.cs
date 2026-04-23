using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Persistence.seeders;

public static class PaymentMediumTypesSeeder
{
    private static readonly string[] Types =
    [
        "Efectivo",
        "Tarjeta",
        "Transferencia"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.PaymentMediumTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in Types)
        {
            if (existing.Contains(name))
                continue;

            await db.PaymentMediumTypes.AddAsync(new PaymentMediumTypesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

