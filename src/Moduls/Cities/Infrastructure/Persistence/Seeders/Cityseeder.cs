using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Seeders;

public sealed class CitySeeder
{
    private readonly AppDbContext _context;

    private static readonly (string City, string Region)[] _cities = new[]
    {
        ("Medellín", "Antioquia"),
        ("Bogotá", "Cundinamarca"),
        ("Cali", "Valle del Cauca"),
        ("Barranquilla", "Atlántico"),
        ("Bucaramanga", "Santander"),
        ("Miami", "Florida"),
        ("Orlando", "Florida"),
        ("Houston", "Texas"),
        ("Paris", "Île-de-France"),
        ("Madrid", "Madrid"),
        ("Barcelona", "Cataluña")
    };

    public CitySeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var regions = await _context.Regions.ToDictionaryAsync(r => r.Name, r => r.Id, StringComparer.OrdinalIgnoreCase);
        var existingCities = await _context.Cities.Select(c => c.Name).ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var (cityName, regionName) in _cities)
        {
            if (existingCities.Contains(cityName)) continue;
            if (regions.TryGetValue(regionName, out var regionId))
            {
                await _context.Cities.AddAsync(new CityEntity
                {
                    Name = cityName,
                    RegionId = regionId,
                    RegionId1 = regionId
                });
            }
        }
        await _context.SaveChangesAsync();
    }
}
