using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// vuelos con mayor ocupación;
public sealed class GetAvailableFlightsUseCase
{
    
    private readonly AppDbContext _context;

    public GetAvailableFlightsUseCase(AppDbContext context) => _context = context;

    public async Task<IEnumerable<FlightOccupancyDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var flights  = await _context.Flights.AsNoTracking().ToListAsync(ct);
        var routes   = await _context.Routes.AsNoTracking().ToListAsync(ct);
        var airports = await _context.Airports.AsNoTracking().ToListAsync(ct);

        var result =
            from f in flights
            join r  in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            where f.AvailableSeats > 0 // solo los que tienen espacio
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
            .OrderBy(x => x.AvailableSeats) // primero los más llenos
            .ToList();
    }
}