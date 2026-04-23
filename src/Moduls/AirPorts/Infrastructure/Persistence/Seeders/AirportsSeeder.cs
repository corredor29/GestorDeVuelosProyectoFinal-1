using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Seeders;

public sealed class AirportsSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string IataCode, string? IcaoCode, string CityName)[] _airports =
    {
        ("Aeropuerto Internacional El Dorado", "BOG", "SKBO", "Bogotá"),
        ("Aeropuerto Internacional José María Córdova", "MDE", "SKRG", "Medellín"),
        ("Miami International Airport", "MIA", "KMIA", "Miami"),
        ("Orlando International Airport", "MCO", "KMCO", "Orlando"),
        ("Adolfo Suárez Madrid-Barajas", "MAD", "LEMD", "Madrid")
    };

    public AirportsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var cities = await _context.Cities
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existingIataCodes = await _context.Airports
            .Select(x => x.IataCode)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, iataCode, icaoCode, cityName) in _airports)
        {
            if (existingIataCodes.Contains(iataCode))
                continue;

            if (!cities.TryGetValue(cityName, out var cityId))
                continue;

            await _context.Airports.AddAsync(new AirportEntity
            {
                Name = name,
                IataCode = iataCode,
                IcaoCode = icaoCode,
                CityId = cityId
            });
        }

        await _context.SaveChangesAsync();
    }
}
