using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Persistence.seeders;

public sealed class AircraftSeeder
{
    private readonly AppDbContext _context;

    public AircraftSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var modelIds = await _context.AircraftModels
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        var airlineIds = await _context.Airlines
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (modelIds.Count == 0 || airlineIds.Count == 0)
            return;

        var seedData = new[]
        {
            new { Registration = "HK-1001", ModelId = modelIds[0], AirlineId = airlineIds[0], ManufacturedDate = new DateTime(2018, 5, 12), IsActive = true },
            new { Registration = "HK-1002", ModelId = modelIds[Math.Min(1, modelIds.Count - 1)], AirlineId = airlineIds[Math.Min(1, airlineIds.Count - 1)], ManufacturedDate = new DateTime(2019, 8, 22), IsActive = true },
            new { Registration = "HK-1003", ModelId = modelIds[Math.Min(2, modelIds.Count - 1)], AirlineId = airlineIds[Math.Min(2, airlineIds.Count - 1)], ManufacturedDate = new DateTime(2017, 2, 10), IsActive = true }
        };

        var existingRegistrations = await _context.Aircrafts
            .AsNoTracking()
            .Select(x => x.Registration)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var seed in seedData)
        {
            if (existingRegistrations.Contains(seed.Registration))
                continue;

            await _context.Aircrafts.AddAsync(new AircraftEntity
            {
                AircraftModelId = seed.ModelId,
                AirlinesId = seed.AirlineId,
                Registration = seed.Registration,
                DateManufactured = seed.ManufacturedDate,
                IsActive = seed.IsActive
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
