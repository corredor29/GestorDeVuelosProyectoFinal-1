using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Persistence.Seeders;

public sealed class BookingStatusTransitionsSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string FromStatus, string ToStatus)[] Transitions =
    {
        ("Pending", "Confirmed"),
        ("Pending", "Cancelled"),
        ("Pending", "Expired"),
        ("Confirmed", "Cancelled")
    };

    public BookingStatusTransitionsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var statuses = await _context.BookingStatuses
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var existing = await _context.BookingStatusTransitions
            .Select(x => new { x.FromStatusId, x.ToStatusId })
            .ToListAsync(cancellationToken);

        foreach (var item in Transitions)
        {
            if (!statuses.TryGetValue(item.FromStatus, out var fromStatusId))
                continue;

            if (!statuses.TryGetValue(item.ToStatus, out var toStatusId))
                continue;

            if (existing.Any(x => x.FromStatusId == fromStatusId && x.ToStatusId == toStatusId))
                continue;

            await _context.BookingStatusTransitions.AddAsync(new BookingStatusTransitionEntity
            {
                FromStatusId = fromStatusId,
                ToStatusId = toStatusId
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
