using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Persistence.seeders;

public static class CheckinsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Checkins.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var ticketId = await db.Tickets
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var staffId = await db.Staffs
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var seatId = await db.FlightSeats
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var confirmedId = await db.CheckinStates
            .AsNoTracking()
            .Where(x => x.Name == "Confirmado")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticketId is null || staffId is null || seatId is null || confirmedId is null)
            return;

        var now = DateTime.UtcNow;
        await db.Checkins.AddAsync(new CheckinEntity
        {
            TicketId = ticketId.Value,
            StaffId = staffId.Value,
            FlightSeatId = seatId.Value,
            CheckedInAt = now,
            CheckinStatusId = confirmedId.Value,
            BoardingPassNumber = $"BP-{now:HHmmss}"
        }, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
    }
}

