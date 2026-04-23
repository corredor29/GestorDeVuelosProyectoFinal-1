using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Persistence.seeders;

public static class InvoiceItemTypesSeeder
{
    private static readonly string[] Types =
    [
        "Tiquete",
        "Impuesto",
        "Servicio",
        "Equipaje"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.InvoiceItemTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in Types)
        {
            if (existing.Contains(name))
                continue;

            await db.InvoiceItemTypes.AddAsync(new InvoiceItemTypesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

