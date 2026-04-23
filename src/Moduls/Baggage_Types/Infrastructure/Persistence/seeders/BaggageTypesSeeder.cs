using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Persistence.seeders;

public static class BaggageTypesSeeder
{
    private static readonly (string Name, decimal MaxWeightKg, decimal BasePrice)[] Types =
    [
        ("Equipaje de mano", 10m, 0m),
        ("Equipaje bodega", 23m, 0m),
        ("Sobrecupo", 32m, 50_000m)
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.BaggageTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (name, max, price) in Types)
        {
            if (existing.Contains(name))
                continue;

            await db.BaggageTypes.AddAsync(new BaggageTypesEntity
            {
                Name = name,
                MaxWeightKg = max,
                BasePrice = price
            }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

