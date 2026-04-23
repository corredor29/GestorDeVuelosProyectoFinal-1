using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Seeders;

public sealed class AvailabilityStatesSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string From, string To)[] EnglishToSpanish =
    {
        ("Available", "Disponible"),
        ("Assigned", "Asignado"),
        ("Vacation", "Vacaciones"),
        ("Leave", "Permiso"),
        ("Inactive", "Inactivo")
    };

    private static readonly string[] SpanishStates =
    {
        "Disponible",
        "Asignado",
        "Vacaciones",
        "Permiso",
        "Inactivo"
    };

    public AvailabilityStatesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        foreach (var (from, to) in EnglishToSpanish)
        {
            var entity = await _context.AvailabilityStates.FirstOrDefaultAsync(x => x.Name == from);
            if (entity is not null && entity.Name != to)
                entity.Name = to;
        }

        await _context.SaveChangesAsync();

        var existing = await _context.AvailabilityStates
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var name in SpanishStates)
        {
            if (existing.Contains(name))
                continue;

            await _context.AvailabilityStates.AddAsync(new AvailabilityStateEntity { Name = name });
        }

        await _context.SaveChangesAsync();
    }
}
