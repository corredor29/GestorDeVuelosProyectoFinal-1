using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Persistence.Seeders;

public sealed class BookingFlightsSeeder
{
    private readonly AppDbContext _context;

    public BookingFlightsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.BookingFlights.AnyAsync(cancellationToken))
            return;

        var bookingIds = await _context.Bookings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        var flightIds = await _context.Flights
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (bookingIds.Count == 0 || flightIds.Count == 0)
            return;

        var items = new List<BookingFlightsEntity>
        {
            new()
            {
                BookingId = bookingIds[0],
                FlightId = flightIds[0],
                PartialAmount = 150.00m
            }
        };

        if (bookingIds.Count > 1 && flightIds.Count > 1)
        {
            items.Add(new BookingFlightsEntity
            {
                BookingId = bookingIds[1],
                FlightId = flightIds[1],
                PartialAmount = 325.50m
            });
        }

        if (bookingIds.Count > 2 && flightIds.Count > 2)
        {
            items.Add(new BookingFlightsEntity
            {
                BookingId = bookingIds[2],
                FlightId = flightIds[2],
                PartialAmount = 210.75m
            });
        }

        await _context.BookingFlights.AddRangeAsync(items, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
