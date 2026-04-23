using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Seeders;

public sealed class PersonalSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string DocumentNumber, string PositionName, string? AirlineIata, string? AirportIata, DateTime HireDate, bool IsActive)[] _staff =
    {
        ("1001", "Pilot", "AVA", "BOG", new DateTime(2023, 1, 10), true),
        ("1002", "Check-In Agent", null, "MDE", new DateTime(2024, 2, 1), true)
    };

    public PersonalSeeder(AppDbContext context)
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
        var positions = await _context.PersonalPositions.ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var airlines = await _context.Airlines.ToDictionaryAsync(x => x.IataCode, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var airports = await _context.Airports.ToDictionaryAsync(x => x.IataCode, x => x.Id, StringComparer.OrdinalIgnoreCase);
        var existingPersonIds = await _context.Staffs.Select(x => x.PersonId).ToHashSetAsync();

        foreach (var item in _staff)
        {
            if (!peopleByDocument.TryGetValue(item.DocumentNumber, out var personId))
                continue;
            if (!positions.TryGetValue(item.PositionName, out var positionId))
                continue;
            if (existingPersonIds.Contains(personId))
                continue;

            int? airlineId = null;
            if (!string.IsNullOrWhiteSpace(item.AirlineIata) && airlines.TryGetValue(item.AirlineIata, out var aId))
                airlineId = aId;

            int? airportId = null;
            if (!string.IsNullOrWhiteSpace(item.AirportIata) && airports.TryGetValue(item.AirportIata, out var apId))
                airportId = apId;

            await _context.Staffs.AddAsync(new StaffEntity
            {
                PersonId = personId,
                PositionId = positionId,
                AirlineId = airlineId,
                AirportId = airportId,
                HireDate = item.HireDate,
                IsActive = item.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }
}
