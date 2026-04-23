using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Seeders;

public sealed class CountrySeeder
{
    private readonly AppDbContext _context;

    // (name, iso_code, continent_name)
    private static readonly (string Name, string IsoCode, string ContinentName)[] _countries = new[]
    {
        // Americas
        ("Colombia",       "COL", "Americas"),
        ("United States",  "USA", "Americas"),
        ("Mexico",         "MEX", "Americas"),
        ("Brazil",         "BRA", "Americas"),
        ("Argentina",      "ARG", "Americas"),
        ("Canada",         "CAN", "Americas"),
        // Europe
        ("Spain",          "ESP", "Europe"),
        ("France",         "FRA", "Europe"),
        ("Germany",        "DEU", "Europe"),
        ("Italy",          "ITA", "Europe"),
        ("United Kingdom", "GBR", "Europe"),
        // Asia
        ("Japan",          "JPN", "Asia"),
        ("China",          "CHN", "Asia"),
        ("India",          "IND", "Asia"),
        ("South Korea",    "KOR", "Asia"),
        // Africa
        ("South Africa",   "ZAF", "Africa"),
        ("Nigeria",        "NGA", "Africa"),
        ("Egypt",          "EGY", "Africa"),
        // Oceania
        ("Australia",      "AUS", "Oceania"),
        ("New Zealand",    "NZL", "Oceania"),
    };

    public CountrySeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Load existing continent IDs by name
        var continents = await _context.Continents
            .ToDictionaryAsync(c => c.Name, c => c.Id);

        var existingIsoCodes = await _context.Countries
            .Select(c => c.IsoCode)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, isoCode, continentName) in _countries)
        {
            if (existingIsoCodes.Contains(isoCode)) continue;

            if (!continents.TryGetValue(continentName, out var continentId))
                continue; // skip if continent not seeded yet

            await _context.Countries.AddAsync(new CountryEntity
            {
                Name = name,
                IsoCode = isoCode,
                ContinentId = continentId
            });
        }

        await _context.SaveChangesAsync();
    }
}