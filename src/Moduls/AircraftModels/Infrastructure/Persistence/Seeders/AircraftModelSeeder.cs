using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Persistence.Seeders;

public sealed class AircraftModelSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Manufacturer, string ModelName, int MaxCapacity, decimal? Weight, decimal? Fuel, int? Speed, int? Altitude)[] SeedData =
    {
        ("Boeing", "737-800", 189, 79015m, 2600m, 842, 41000),
        ("Airbus", "A320", 180, 77000m, 2400m, 828, 39000),
        ("Boeing", "787-9", 296, 254000m, 5600m, 903, 43000),
        ("Airbus", "A380", 555, 575000m, 13000m, 903, 43000),
        ("Embraer", "E190", 100, 51800m, 1700m, 870, 41000)
    };

    public AircraftModelSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var manufacturers = await _context.AircraftManufacturers
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var existing = await _context.AircraftModels
            .AsNoTracking()
            .Select(x => new { x.AircraftManufacturerId, x.ModelName })
            .ToListAsync(cancellationToken);

        foreach (var seed in SeedData)
        {
            if (!manufacturers.TryGetValue(seed.Manufacturer, out var manufacturerId))
                continue;

            var duplicate = existing.Any(x =>
                x.AircraftManufacturerId == manufacturerId &&
                string.Equals(x.ModelName, seed.ModelName, StringComparison.OrdinalIgnoreCase));

            if (duplicate)
                continue;

            await _context.AircraftModels.AddAsync(new AircraftModelsEntity
            {
                AircraftManufacturerId = manufacturerId,
                ModelName = seed.ModelName,
                MaxCapacity = seed.MaxCapacity,
                MaxTakeoffWeightKg = seed.Weight,
                FuelConsumptionKgH = seed.Fuel,
                CruiseSpeedKmh = seed.Speed,
                CruiseAltitudeFt = seed.Altitude
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
