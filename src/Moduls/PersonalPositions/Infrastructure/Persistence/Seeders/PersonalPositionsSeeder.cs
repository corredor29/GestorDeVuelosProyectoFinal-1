using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Seeders;

public sealed class PersonalPositionsSeeder
{
    private readonly AppDbContext _context;

    private static readonly string[] _positions =
    {
        "Pilot",
        "Copilot",
        "Cabin Chief",
        "Check-In Agent",
        "Administrative"
    };

    public PersonalPositionsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var existing = await _context.PersonalPositions.Select(x => x.Name).ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var name in _positions)
        {
            if (existing.Contains(name))
                continue;

            await _context.PersonalPositions.AddAsync(new PersonalPositionEntity
            {
                Name = name
            });
        }

        await _context.SaveChangesAsync();
    }
}
