using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Persistence.seeders;

public static class BaggageSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Baggages.AsNoTracking().AnyAsync(cancellationToken))
            return;

        var checkinId = await db.Checkins
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var baggageTypeId = await db.BaggageTypes
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (checkinId is null || baggageTypeId is null)
            return;

        await db.Baggages.AddAsync(new BaggageEntity
        {
            CheckinId = checkinId.Value,
            BaggageTypeId = baggageTypeId.Value,
            WeightKg = 8m,
            ChargedPrice = 0m
        }, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);
    }
}

