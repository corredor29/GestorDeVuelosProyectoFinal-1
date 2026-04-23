using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Persistence.seeders;

public static class CheckinStatesSeeder
{
    private static readonly string[] States =
    [
        "Pendiente",
        "Confirmado",
        "Cancelado"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.CheckinStates
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in States)
        {
            if (existing.Contains(name))
                continue;

            await db.CheckinStates.AddAsync(new CheckinStatesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

