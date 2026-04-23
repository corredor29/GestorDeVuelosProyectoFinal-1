using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Persistence.seeders;

public static class TicketsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Tickets.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var passengerReservationId = await db.PassengerReservations
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var issuedStateId = await db.TicketStates
            .AsNoTracking()
            .Where(x => x.Name == "Emitido")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (passengerReservationId is null || issuedStateId is null)
            return;

        var now = DateTime.UtcNow;
        await db.Tickets.AddAsync(new TicketEntity
        {
            Code = $"TKT-{now:yyyyMMddHHmmss}",
            IssueDate = now,
            CreatedAt = now,
            UpdatedAt = now,
            PassengerReservation_Id = passengerReservationId.Value,
            TicketState_Id = issuedStateId.Value
        }, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
    }
}

