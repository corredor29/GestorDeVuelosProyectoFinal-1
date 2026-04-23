using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Persistence.seeders;

public sealed class FlightSeatsSeeder
{
    private readonly AppDbContext _context;

    public FlightSeatsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Obtenemos IDs necesarios para las relaciones
        var flightIds = await _context.Flights.AsNoTracking().Select(x => x.Id).Take(2).ToListAsync(cancellationToken);
        var cabinTypeIds = await _context.CabinTypes.AsNoTracking().Select(x => x.Id).Take(2).ToListAsync(cancellationToken);
        var locationTypeIds = await _context.SeatLocationTypes.AsNoTracking().Select(x => x.Id).Take(3).ToListAsync(cancellationToken);

        if (!flightIds.Any() || !cabinTypeIds.Any() || !locationTypeIds.Any())
            return;

        var seedData = new[]
        {
            new { FlightId = flightIds[0], CabinId = cabinTypeIds[0], LocId = locationTypeIds[0], Code = "1A", Occupied = false },
            new { FlightId = flightIds[0], CabinId = cabinTypeIds[0], LocId = locationTypeIds[Math.Min(1, locationTypeIds.Count-1)], Code = "1B", Occupied = true },
            new { FlightId = flightIds[Math.Min(1, flightIds.Count-1)], CabinId = cabinTypeIds[Math.Min(1, cabinTypeIds.Count-1)], LocId = locationTypeIds[Math.Min(2, locationTypeIds.Count-1)], Code = "10C", Occupied = false }
        };

        var existingCodes = await _context.FlightSeats
            .AsNoTracking()
            .Select(x => x.Code)
            .ToHashSetAsync(cancellationToken);

        foreach (var seed in seedData)
        {
            if (existingCodes.Contains(seed.Code)) continue;

            await _context.FlightSeats.AddAsync(new FlightSeatEntity
            {
                FlightId = seed.FlightId,
                CabinTypeId = seed.CabinId,
                SeatLocationTypeId = seed.LocId,
                Code = seed.Code,
                IsOccupied = seed.Occupied
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}