using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Persistence.seeders;

public static class CardTypesSeeder
{
    private static readonly string[] Types =
    [
        "VISA",
        "MASTERCARD"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.CardTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in Types)
        {
            if (existing.Contains(name))
                continue;

            await db.CardTypes.AddAsync(new CardTypesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

