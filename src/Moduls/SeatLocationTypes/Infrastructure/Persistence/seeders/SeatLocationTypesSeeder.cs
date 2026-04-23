using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Persistence.seeders;

public sealed class SeatLocationTypesSeeder
{
    private readonly AppDbContext _context;

    private static readonly string[] DefaultSeatLocationTypes =
    {
        "Económico",
        "Business",
        "Primera",
        "VIP"
    };

    public SeatLocationTypesSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingNames = await _context.SeatLocationTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var normalized = existingNames
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var seatLocationType in DefaultSeatLocationTypes)
        {
            if (normalized.Contains(seatLocationType))
                continue;

            await _context.SeatLocationTypes.AddAsync(new SeatLocationTypesEntity
            {
                Name = seatLocationType
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
