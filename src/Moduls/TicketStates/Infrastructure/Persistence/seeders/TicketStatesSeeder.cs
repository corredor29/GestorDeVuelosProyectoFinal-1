using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Persistence.seeders;

public static class TicketStatesSeeder
{
    private static readonly string[] States =
    [
        "Emitido",
        "Usado",
        "Cancelado"
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        var existing = await db.TicketStates
            .AsNoTracking()
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in States)
        {
            if (existing.Contains(name))
                continue;

            await db.TicketStates.AddAsync(new TicketStatesEntity { Name = name }, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

