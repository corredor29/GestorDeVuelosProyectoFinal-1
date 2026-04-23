using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// tiquetes emitidos por rango de fechas.
public sealed class GetTicketsByDateRangeUseCase
{
    private readonly AppDbContext _context;

    public GetTicketsByDateRangeUseCase(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TicketsByDateDto>> ExecuteAsync(
        DateTime fromDate, DateTime toDate, CancellationToken ct = default)
    {
        if (toDate < fromDate)
            throw new ArgumentException("La fecha final no puede ser menor a la inicial.");

        // Confirma estos nombres con tu AppDbContext:
        // _context.Tickets           → DbSet<TicketEntity>
        // _context.BookingFlights    → DbSet<BookingFlightsEntity>
        // _context.PassengerReservations → DbSet<PassengerReservationsEntity>  ← confirmar
        // _context.TicketStatuses    → ¿existe este DbSet?                     ← confirmar

        var tickets = await _context.tickets.AsNoTracking().ToListAsync(ct);
        var passengerRes = await _context.PassengerReservations.AsNoTracking().ToListAsync(ct); // ← nombre a confirmar
        var bookingFlights = await _context.BookingFlights.AsNoTracking().ToListAsync(ct);
        var flights = await _context.Flights.AsNoTracking().ToListAsync(ct);
        var routes = await _context.Routes.AsNoTracking().ToListAsync(ct);
        var airports = await _context.Airports.AsNoTracking().ToListAsync(ct);

        var result =
            from t  in tickets
            where t.IssueDate >= fromDate && t.IssueDate <= toDate
            join pr in passengerRes on t.PassengerReservation_Id equals pr.Id
            join bf in bookingFlights on pr.Flight_Reservation_Id equals bf.Id
            join f  in flights on bf.FlightId equals f.Id
            join r  in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            select new TicketsByDateDto(
                t.Code,
                f.FlightCode,
                ao.IataCode,
                ad.IataCode,
                t.IssueDate,
                t.TicketState_Id.ToString());   // si tienes DbSet<TicketStatusEntity> lo joinamos también

        return result
            .OrderByDescending(x => x.IssuedAt)
            .ToList();
    }
}
