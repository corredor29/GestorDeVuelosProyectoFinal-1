using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Persistence.seeders;

public static class PassengerReservationsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.PassengerReservations.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var flightReservationId = await db.FlightReservations
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var passengerId = await db.Passengers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (flightReservationId is null || passengerId is null)
            return;

        await db.PassengerReservations.AddAsync(new PassengerReservationsEntity
        {
            Flight_Reservation_Id = flightReservationId.Value,
            Passenger_Id = passengerId.Value
        }, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
    }
}

