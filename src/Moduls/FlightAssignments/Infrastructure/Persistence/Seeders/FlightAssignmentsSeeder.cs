using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Persistence.Seeders;

public sealed class FlightAssignmentsSeeder
{
    private const string AssignedStateName = "Assigned";

    private readonly AppDbContext _context;

    public FlightAssignmentsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var assignedStateId = await _context.AvailabilityStates.AsNoTracking()
            .Where(x => x.Name.ToLower() == AssignedStateName.ToLower())
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (assignedStateId is null)
            return;

        var flights = await _context.Flights.AsNoTracking()
            .OrderBy(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        var staffs = await _context.Staffs.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .Take(6)
            .ToListAsync(cancellationToken);

        var roles = await _context.FlightRoles.AsNoTracking()
            .OrderBy(x => x.Id)
            .Take(4)
            .ToListAsync(cancellationToken);

        if (flights.Count == 0 || staffs.Count == 0 || roles.Count == 0)
            return;

        var existingAssignments = await _context.FlightAssignments.AsNoTracking()
            .Select(x => new { x.FlightId, x.StaffId })
            .ToListAsync(cancellationToken);

        var existingAvailability = await _context.StaffAvailabilities.AsNoTracking()
            .Select(x => new { x.StaffId, x.StartsAt, x.EndsAt, x.AvailabilityStatusId, x.Notes })
            .ToListAsync(cancellationToken);

        var roleIndex = 0;

        foreach (var flight in flights)
        {
            foreach (var staff in staffs)
            {
                var alreadyAssigned = existingAssignments.Any(x => x.FlightId == flight.Id && x.StaffId == staff.Id);
                if (alreadyAssigned)
                    continue;

                var hasBlockingAvailability = existingAvailability.Any(x =>
                    x.StaffId == staff.Id &&
                    x.StartsAt < flight.EstimatedArrivalAt &&
                    x.EndsAt > flight.DepartureAt &&
                    x.AvailabilityStatusId != assignedStateId.Value);

                if (hasBlockingAvailability)
                    continue;

                var role = roles[roleIndex % roles.Count];
                roleIndex++;

                await _context.FlightAssignments.AddAsync(new FlightAssignmentEntity
                {
                    FlightId = flight.Id,
                    StaffId = staff.Id,
                    FlightRoleId = role.Id
                }, cancellationToken);

                var note = $"Asignacion de tripulacion para vuelo {flight.Id}";
                var alreadyBlocked = existingAvailability.Any(x =>
                    x.StaffId == staff.Id &&
                    x.AvailabilityStatusId == assignedStateId.Value &&
                    x.StartsAt == flight.DepartureAt &&
                    x.EndsAt == flight.EstimatedArrivalAt &&
                    x.Notes == note);

                if (!alreadyBlocked)
                {
                    await _context.StaffAvailabilities.AddAsync(new GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities.StaffAvailabilityEntity
                    {
                        StaffId = staff.Id,
                        AvailabilityStatusId = assignedStateId.Value,
                        StartsAt = flight.DepartureAt,
                        EndsAt = flight.EstimatedArrivalAt,
                        Notes = note
                    }, cancellationToken);
                }

                break;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
