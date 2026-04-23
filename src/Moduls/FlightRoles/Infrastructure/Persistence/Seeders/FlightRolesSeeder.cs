using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Persistence.Seeders;

public sealed class FlightRolesSeeder
{
    private readonly AppDbContext _context;

    private static readonly string[] DefaultRoles =
    {
        "Captain",
        "Copilot",
        "Cabin Chief",
        "Flight Attendant"
    };

    public FlightRolesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingNames = await _context.FlightRoles
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var normalized = existingNames
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var role in DefaultRoles)
        {
            if (normalized.Contains(role))
                continue;

            await _context.FlightRoles.AddAsync(new FlightRolesEntity
            {
                Name = role
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
