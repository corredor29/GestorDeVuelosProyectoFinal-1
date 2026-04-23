using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Persistence.seeders;

public static class CardIssuersSeeder
{
    private static readonly (string Name, string IssuerNumber)[] Issuers =
    [
        ("Banco Demo", "000001"),
        ("Tarjetas Demo", "000002")
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.CardIssuers
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (name, issuerNumber) in Issuers)
        {
            if (existing.Contains(name))
                continue;

            await db.CardIssuers.AddAsync(new CardIssuerEntity
            {
                Name = name,
                IssuerNumber = issuerNumber
            }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

