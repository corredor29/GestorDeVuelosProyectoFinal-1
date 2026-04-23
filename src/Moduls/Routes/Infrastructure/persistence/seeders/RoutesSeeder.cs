using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Persistence.Seeders;

public sealed class RoutesSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string OriginIata, string DestinationIata, int? DistanceKm, int? EstimatedDurationMin)[] _routes =
    {
        ("BOG", "MDE", 216, 55),
        ("MDE", "BOG", 216, 55),
        ("BOG", "MIA", 2435, 225),
        ("MIA", "BOG", 2435, 225),
        ("MDE", "MAD", 8020, 540),
        ("MAD", "MDE", 8020, 540),
        ("MAD", "BOG", 8030, 600),
        ("BOG", "MAD", 8030, 600)
    };

    public RoutesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var airports = await _context.Airports
            .ToDictionaryAsync(x => x.IataCode, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existingRoutes = await _context.Routes
            .Select(x => new { x.OriginAirportId, x.DestinationAirportId })
            .ToListAsync();

        foreach (var item in _routes)
        {
            if (!airports.TryGetValue(item.OriginIata, out var originAirportId))
                continue;

            if (!airports.TryGetValue(item.DestinationIata, out var destinationAirportId))
                continue;

            if (existingRoutes.Any(x => x.OriginAirportId == originAirportId && x.DestinationAirportId == destinationAirportId))
                continue;

            await _context.Routes.AddAsync(new RouteEntity
            {
                OriginAirportId = originAirportId,
                DestinationAirportId = destinationAirportId,
                DistanceKm = item.DistanceKm,
                EstimatedDurationMin = item.EstimatedDurationMin
            });
        }

        await _context.SaveChangesAsync();
    }
}
