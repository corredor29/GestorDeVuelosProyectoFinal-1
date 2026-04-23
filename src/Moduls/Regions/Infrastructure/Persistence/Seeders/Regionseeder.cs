using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Seeders;

public sealed class RegionSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string Type, string CountryIso)[] _regions = new[]
    {
        ("Antioquia", "Departamento", "COL"),
        ("Cundinamarca", "Departamento", "COL"),
        ("Valle del Cauca", "Departamento", "COL"),
        ("Atlántico", "Departamento", "COL"),
        ("Santander", "Departamento", "COL"), // ¡Agregué tu tierra!
        ("California", "State", "USA"),
        ("Florida", "State", "USA"),
        ("New York", "State", "USA"),
        ("Texas", "State", "USA"),
        ("Jalisco", "Estado", "MEX"),
        ("Ciudad de México", "Estado", "MEX"),
        ("Nuevo León", "Estado", "MEX"),
        ("Madrid", "Comunidad Autónoma", "ESP"),
        ("Cataluña", "Comunidad Autónoma", "ESP"),
        ("Andalucía", "Comunidad Autónoma", "ESP"),
        ("Île-de-France", "Région", "FRA"),
        ("Provence", "Région", "FRA"),
        ("São Paulo", "Estado", "BRA"),
        ("Rio de Janeiro", "Estado", "BRA"),
        ("Buenos Aires", "Provincia", "ARG"),
        ("Córdoba", "Provincia", "ARG"),
    };

    public RegionSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var countries = await _context.Countries.ToDictionaryAsync(c => c.IsoCode, c => c.Id);
        var existingNames = await _context.Regions.Select(r => r.Name).ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, type, countryIso) in _regions)
        {
            if (existingNames.Contains(name)) continue;
            if (!countries.TryGetValue(countryIso, out var countryId)) continue;

            await _context.Regions.AddAsync(new RegionEntity
            {
                Name = name,
                Type = type,
                CountryId = countryId
            });
        }
        await _context.SaveChangesAsync();
    }
}