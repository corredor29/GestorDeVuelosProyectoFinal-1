using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Seeders;

public sealed class StreetTypeSeeder
{
    private readonly AppDbContext _context;

    private static readonly string[] DefaultTypes =
    {
        "Calle", "Carrera", "Avenida", "Diagonal", "Transversal",
        "Pasaje", "Paseo", "Autopista", "Circunvalar", "Otro"
    };

    public StreetTypeSeeder(AppDbContext context) => _context = context;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existing = await _context.StreetTypes
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in DefaultTypes)
        {
            if (existing.Contains(name))
                continue;

            await _context.StreetTypes.AddAsync(new StreetTypeEntity { Name = name }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
