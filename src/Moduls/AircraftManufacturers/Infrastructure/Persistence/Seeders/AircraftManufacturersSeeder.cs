using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Persistence.Seeders;

public sealed class AircraftManufacturersSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string Country)[] SeedData =
    {
        ("Airbus", "France"),
        ("Boeing", "United States"),
        ("Embraer", "Brazil"),
        ("Bombardier", "Canada"),
        ("ATR", "France"),
        ("Cessna", "United States")
    };

    public AircraftManufacturersSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingNames = await _context.AircraftManufacturers
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (name, country) in SeedData)
        {
            if (existingNames.Contains(name))
                continue;

            await _context.AircraftManufacturers.AddAsync(new AircraftManufacturerEntity
            {
                Name = name,
                Country = country
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
