using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Persistence.Seeders;

public sealed class SeasonsSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string? Description, decimal PriceFactor)[] _seasons =
    {
        ("Low", "Temporada baja con menor demanda", 0.9000m),
        ("Mid", "Temporada media con demanda estable", 1.0000m),
        ("High", "Temporada alta con alta demanda", 1.2000m),
        ("Christmas", "Temporada navidena", 1.3500m),
        ("Easter", "Temporada de semana santa", 1.1500m)
    };

    public SeasonsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var existingNames = await _context.Seasons
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var item in _seasons)
        {
            if (existingNames.Contains(item.Name))
                continue;

            await _context.Seasons.AddAsync(new SeasonEntity
            {
                Name = item.Name,
                Description = item.Description,
                PriceFactor = item.PriceFactor
            });
        }

        await _context.SaveChangesAsync();
    }
}
