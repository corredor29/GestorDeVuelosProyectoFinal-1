using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Infrastructure.Persistence.Seeders;

public sealed class CabinTypeSeeder
{
    private readonly AppDbContext _context;

    // Listado predefinido de tipos de cabina estándar
    private static readonly string[] _cabinTypes = new[]
    {
        "Economy",
        "Business",
        "First Class",
        "VIP"
    };

    public CabinTypeSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Obtenemos los nombres existentes para evitar duplicados (la columna es UNIQUE)
        var existingNames = await _context.Set<CabinTypeEntity>()
            .Select(c => c.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var name in _cabinTypes)
        {
            if (existingNames.Contains(name)) continue;

            await _context.Set<CabinTypeEntity>().AddAsync(new CabinTypeEntity
            {
                Name = name
            });
        }

        await _context.SaveChangesAsync();
    }
}
