using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Persistence.seeders;

public static class FlightReservationsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.FlightReservations.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var bookingFlightId = await db.BookingFlights
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (bookingFlightId is null)
            return;

        await db.FlightReservations.AddAsync(new FlightReservationsEntity { BookingFlightId = bookingFlightId.Value }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}

