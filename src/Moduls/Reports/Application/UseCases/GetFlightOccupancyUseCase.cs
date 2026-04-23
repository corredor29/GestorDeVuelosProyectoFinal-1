using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// vuelos con asientos disponibles;
public sealed class GetFlightOccupancyUseCase
{
    private readonly AppDbContext _context;
    public GetFlightOccupancyUseCase(AppDbContext context) => _context = context;

    public async Task<IEnumerable<FlightOccupancyDto>> ExecuteAsync(CancellationToken ct = default)
    {
        // Traemos las tres tablas que necesitamos
        var flights = await _context.Flights.AsNoTracking().ToListAsync(ct);
        var routes  = await _context.Routes.AsNoTracking().ToListAsync(ct);
        var airports = await _context.Airports.AsNoTracking().ToListAsync(ct);

        // Join manual: flight → route → airport origen y destino
        var result =
            from f in flights
            join r  in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            where f.TotalCapacity > 0
            let occupied = f.TotalCapacity - f.AvailableSeats
            select new FlightOccupancyDto(
                f.FlightCode,
                ao.IataCode,
                ad.IataCode,
                f.TotalCapacity,
                occupied,
                f.AvailableSeats,
                Math.Round((double)occupied / f.TotalCapacity * 100, 1));

        return result
            .OrderByDescending(x => x.OccupancyPercent)
            .ToList();
    }
}