using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Persistence.Seeders;

public sealed class RatesSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string OriginIata, string DestinationIata, string CabinTypeName, string PassengerTypeName, string SeasonName, decimal BasePrice, DateOnly? ValidFrom, DateOnly? ValidUntil)[] _rates =
    {
        ("BOG", "MDE", "Economy", "Adult", "Low", 120.00m, new DateOnly(2026, 1, 1), null),
        ("MDE", "BOG", "Economy", "Adult", "Low", 120.00m, new DateOnly(2026, 1, 1), null),
        ("BOG", "MIA", "Business", "Adult", "High", 850.00m, new DateOnly(2026, 1, 1), null),
        ("MIA", "BOG", "Business", "Adult", "High", 850.00m, new DateOnly(2026, 1, 1), null),
        ("MDE", "MAD", "Economy", "Adult", "Low", 520.00m, new DateOnly(2026, 1, 1), null),
        ("MAD", "MDE", "Economy", "Adult", "Low", 520.00m, new DateOnly(2026, 1, 1), null),
        ("BOG", "MAD", "Economy", "Adult", "Low", 680.00m, new DateOnly(2026, 1, 1), null),
        ("MAD", "BOG", "Economy", "Child", "Christmas", 700.00m, new DateOnly(2026, 12, 1), new DateOnly(2027, 1, 15))
    };

    public RatesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var routes = await (
            from route in _context.Routes
            join origin in _context.Airports on route.OriginAirportId equals origin.Id
            join destination in _context.Airports on route.DestinationAirportId equals destination.Id
            select new
            {
                route.Id,
                Key = origin.IataCode + "->" + destination.IataCode
            }
        ).ToDictionaryAsync(x => x.Key, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var cabinTypes = await _context.CabinTypes.ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var passengerTypes = await _context.PassengerTypes.ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var seasons = await _context.Seasons.ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var item in _rates)
        {
            var routeKey = $"{item.OriginIata}->{item.DestinationIata}";

            if (!routes.TryGetValue(routeKey, out var routeId))
                continue;
            if (!cabinTypes.TryGetValue(item.CabinTypeName, out var cabinTypeId))
                continue;
            if (!passengerTypes.TryGetValue(item.PassengerTypeName, out var passengerTypeId))
                continue;
            if (!seasons.TryGetValue(item.SeasonName, out var seasonId))
                continue;

            var exists = await _context.Rates.AnyAsync(x =>
                x.RouteId == routeId &&
                x.CabinTypeId == cabinTypeId &&
                x.PassengerTypeId == passengerTypeId &&
                x.SeasonId == seasonId &&
                x.BasePrice == item.BasePrice &&
                x.ValidFrom == item.ValidFrom &&
                x.ValidUntil == item.ValidUntil);

            if (exists)
                continue;

            await _context.Rates.AddAsync(new RateEntity
            {
                RouteId = routeId,
                CabinTypeId = cabinTypeId,
                PassengerTypeId = passengerTypeId,
                SeasonId = seasonId,
                BasePrice = item.BasePrice,
                ValidFrom = item.ValidFrom,
                ValidUntil = item.ValidUntil
            });
        }

        await _context.SaveChangesAsync();
    }
}
