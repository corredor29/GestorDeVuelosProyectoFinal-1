using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Persistence.Seeders;

public sealed class StaffAvailabilitySeeder
{
    private readonly AppDbContext _context;

    private static readonly (string StaffDocumentNumber, string StatusName, DateTime StartsAt, DateTime EndsAt, string? Notes)[] _availabilities =
    {
        ("1001", "Assigned", new DateTime(2026, 4, 20, 8, 0, 0), new DateTime(2026, 4, 20, 18, 0, 0), "Asignado a operacion de prueba"),
        ("1002", "Vacation", new DateTime(2026, 4, 21, 0, 0, 0), new DateTime(2026, 4, 25, 23, 59, 0), "Vacaciones programadas")
    };

    public StaffAvailabilitySeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var people = await _context.Persons.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();
        var peopleByDocument = people
            .GroupBy(x => x.DocumentNumber, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        var staff = await _context.Staffs.AsNoTracking()
            .ToDictionaryAsync(x => x.PersonId, x => x.Id);

        var statuses = await _context.AvailabilityStates.AsNoTracking()
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existing = await _context.StaffAvailabilities.AsNoTracking()
            .Select(x => new { x.StaffId, x.AvailabilityStatusId, x.StartsAt, x.EndsAt })
            .ToListAsync();

        foreach (var item in _availabilities)
        {
            if (!peopleByDocument.TryGetValue(item.StaffDocumentNumber, out var personId))
                continue;
            if (!staff.TryGetValue(personId, out var staffId))
                continue;
            if (!statuses.TryGetValue(item.StatusName, out var statusId))
                continue;

            var alreadyExists = existing.Any(x =>
                x.StaffId == staffId &&
                x.AvailabilityStatusId == statusId &&
                x.StartsAt == item.StartsAt &&
                x.EndsAt == item.EndsAt);

            if (alreadyExists)
                continue;

            await _context.StaffAvailabilities.AddAsync(new StaffAvailabilityEntity
            {
                StaffId = staffId,
                AvailabilityStatusId = statusId,
                StartsAt = item.StartsAt,
                EndsAt = item.EndsAt,
                Notes = item.Notes
            });
        }

        await _context.SaveChangesAsync();
    }
}
