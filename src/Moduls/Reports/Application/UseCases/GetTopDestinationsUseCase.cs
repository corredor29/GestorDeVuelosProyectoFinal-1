using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// destinos más solicitados;
public sealed class GetTopDestinationsUseCase
{
    private readonly AppDbContext _context;
    public GetTopDestinationsUseCase(AppDbContext context) => _context = context;
    public async Task<IEnumerable<TopDestinationDto>> ExecuteAsync(int top = 5, CancellationToken ct = default)
    {
        var bookingFlights = await _context.BookingFlights.AsNoTracking().ToListAsync(ct);
        var flights = await _context.Flights.AsNoTracking().ToListAsync(ct);
        var routes = await _context.Routes.AsNoTracking().ToListAsync(ct);
        var airports = await _context.Airports.AsNoTracking().ToListAsync(ct);

        // booking_flight → flight → route → aeropuerto destino
        var result =
            from bf in bookingFlights
            join f in flights on bf.FlightId equals f.Id
            join r in routes on f.RouteId equals r.Id
            join ad in airports on r.DestinationAirportId  equals ad.Id
            group bf by new { ad.Id, ad.Name, ad.IataCode } into g
            select new TopDestinationDto(
                g.Key.Name,
                g.Key.IataCode,
                g.Count());

        return result
            .OrderByDescending(x => x.TotalBookings)
            .Take(top)
            .ToList();
    }
}