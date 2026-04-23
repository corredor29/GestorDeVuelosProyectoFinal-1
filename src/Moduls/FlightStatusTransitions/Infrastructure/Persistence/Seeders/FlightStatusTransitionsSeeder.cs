using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Persistence.Seeders;

public sealed class FlightStatusTransitionsSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string FromStatus, string ToStatus)[] _transitions =
    {
        ("Scheduled", "Boarding"),
        ("Scheduled", "Cancelled"),
        ("Scheduled", "Rescheduled"),
        ("Boarding", "In Flight"),
        ("Boarding", "Cancelled"),
        ("In Flight", "Completed"),
        ("In Flight", "Cancelled"),
        ("Rescheduled", "Boarding"),
        ("Rescheduled", "Cancelled")
    };

    public FlightStatusTransitionsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var statuses = await _context.FlightStatuses
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existing = await _context.FlightStatusTransitions
            .Select(x => new { x.FromStatusId, x.ToStatusId })
            .ToListAsync();

        foreach (var item in _transitions)
        {
            if (!statuses.TryGetValue(item.FromStatus, out var fromStatusId))
                continue;

            if (!statuses.TryGetValue(item.ToStatus, out var toStatusId))
                continue;

            if (existing.Any(x => x.FromStatusId == fromStatusId && x.ToStatusId == toStatusId))
                continue;

            await _context.FlightStatusTransitions.AddAsync(new FlightStatusTransitionEntity
            {
                FromStatusId = fromStatusId,
                ToStatusId = toStatusId
            });
        }

        await _context.SaveChangesAsync();
    }
}
