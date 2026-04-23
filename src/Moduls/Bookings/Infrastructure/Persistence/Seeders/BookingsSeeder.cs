using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Persistence.Seeders;

public sealed class BookingsSeeder
{
    private readonly AppDbContext _context;

    public BookingsSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Bookings.AnyAsync(cancellationToken))
            return;

        var clientIds = await _context.Customers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (clientIds.Count == 0)
            return;

        var statuses = await _context.BookingStatuses
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        if (!statuses.TryGetValue("Pending", out var pendingId))
            return;

        var now = DateTime.UtcNow;
        var entities = new List<BookingEntity>
        {
            new()
            {
                ClientId = clientIds[0],
                BookedAt = now.AddHours(-4),
                BookingStatusId = pendingId,
                TotalAmount = 350.00m,
                ExpiresAt = now.AddDays(2),
                CreatedAt = now.AddHours(-4),
                UpdatedAt = now.AddHours(-4)
            }
        };

        if (clientIds.Count > 1 && statuses.TryGetValue("Confirmed", out var confirmedId))
        {
            entities.Add(new BookingEntity
            {
                ClientId = clientIds[1],
                BookedAt = now.AddDays(-1),
                BookingStatusId = confirmedId,
                TotalAmount = 720.50m,
                ExpiresAt = null,
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            });
        }

        if (clientIds.Count > 2 && statuses.TryGetValue("Cancelled", out var cancelledId))
        {
            entities.Add(new BookingEntity
            {
                ClientId = clientIds[2],
                BookedAt = now.AddDays(-2),
                BookingStatusId = cancelledId,
                TotalAmount = 180.75m,
                ExpiresAt = null,
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-1)
            });
        }

        await _context.Bookings.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
