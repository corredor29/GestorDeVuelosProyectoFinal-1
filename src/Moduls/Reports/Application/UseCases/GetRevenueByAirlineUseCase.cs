using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// ingresos estimados por aerolínea;
public sealed class GetRevenueByAirlineUseCase
{
    private readonly AppDbContext _context;
    public GetRevenueByAirlineUseCase(AppDbContext context) => _context = context;
    public async Task<IEnumerable<RevenueByAirlineDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var bookingFlights = await _context.BookingFlights.AsNoTracking().ToListAsync(ct);
        var flights = await _context.Flights.AsNoTracking().ToListAsync(ct);
        var airlines = await _context.Airlines.AsNoTracking().ToListAsync(ct);

        // booking_flight → flight → airline, agrupamos por aerolínea
        var result =
            from bf in bookingFlights
            join f in flights  on bf.FlightId  equals f.Id
            join a in airlines on f.AirlineId  equals a.Id
            group bf by new { a.Id, a.Name, a.IataCode } into g
            select new RevenueByAirlineDto(
                g.Key.Name,
                g.Key.IataCode,
                g.Sum(bf => bf.PartialAmount),
                g.Select(bf => bf.FlightId).Distinct().Count());   // vuelos distintos

        return result
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();
    }
}