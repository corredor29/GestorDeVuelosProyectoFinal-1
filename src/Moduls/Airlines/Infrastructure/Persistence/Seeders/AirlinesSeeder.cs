using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Seeders;

public sealed class AirlinesSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string IataCode, string CountryName, bool IsActive)[] _airlines =
    {
        ("Avianca", "AVA", "Colombia", true),
        ("LATAM Airlines", "LAM", "Brazil", true),
        ("American Airlines", "AAL", "United States", true),
        ("Air France", "AFR", "France", true),
        ("Iberia", "IBE", "Spain", true)
    };

    public AirlinesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var countries = await _context.Countries
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existingCodes = await _context.Airlines
            .Select(x => x.IataCode)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, iataCode, countryName, isActive) in _airlines)
        {
            if (existingCodes.Contains(iataCode))
                continue;

            if (!countries.TryGetValue(countryName, out var countryId))
                continue;

            await _context.Airlines.AddAsync(new AirlineEntity
            {
                Name = name,
                IataCode = iataCode,
                OriginCountryId = countryId,
                IsActive = isActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }
}
