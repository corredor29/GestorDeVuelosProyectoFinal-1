using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Seeders;

public sealed class AirportAirlineSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string AirportIata, string AirlineIata, string? Terminal, DateTime StartDate, DateTime? EndDate, bool IsActive)[] _operations =
    {
        ("BOG", "AVA", "T1", new DateTime(2024, 1, 1), null, true),
        ("MDE", "AVA", "T1", new DateTime(2024, 1, 1), null, true),
        ("MIA", "AAL", "N", new DateTime(2023, 6, 1), null, true),
        ("MAD", "IBE", "4S", new DateTime(2023, 1, 15), null, true),
    };

    public AirportAirlineSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var airports = await _context.Airports.ToDictionaryAsync(x => x.IataCode, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var airlines = await _context.Airlines.ToDictionaryAsync(x => x.IataCode, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var existing = await _context.AirportAirlines.Select(x => new { x.AirportId, x.AirlineId }).ToListAsync();

        foreach (var item in _operations)
        {
            if (!airports.TryGetValue(item.AirportIata, out var airportId))
                continue;
            if (!airlines.TryGetValue(item.AirlineIata, out var airlineId))
                continue;
            if (existing.Any(x => x.AirportId == airportId && x.AirlineId == airlineId))
                continue;

            await _context.AirportAirlines.AddAsync(new AirportAirlineEntity
            {
                AirportId = airportId,
                AirlineId = airlineId,
                Terminal = item.Terminal,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                IsActive = item.IsActive
            });
        }

        await _context.SaveChangesAsync();
    }
}
