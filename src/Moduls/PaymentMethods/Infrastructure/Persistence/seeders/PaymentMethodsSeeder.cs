using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Persistence.seeders;

public static class PaymentMethodsSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        // Requiere tipos de medio de pago (payment_method_types)
        var mediumIds = await db.PaymentMediumTypes
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

        if (mediumIds.Count == 0)
            return;

        // Tipos / emisores de tarjeta son opcionales según el método
        var cardTypeIds = await db.CardTypes.AsNoTracking().ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);
        var issuerIds = await db.CardIssuers.AsNoTracking().ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

        var existing = await db.PaymentMethods
            .AsNoTracking()
            .Select(x => x.DisplayName)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        static int? TryGet(Dictionary<string, int> dict, string key)
            => dict.TryGetValue(key, out var v) ? v : null;

        var seeds = new List<PaymentMethodsEntity>();

        if (mediumIds.TryGetValue("Efectivo", out var cashType) && !existing.Contains("Efectivo"))
        {
            seeds.Add(new PaymentMethodsEntity
            {
                PaymentMethodTypeId = cashType,
                DisplayName = "Efectivo",
                CardTypeId = null,
                CardIssuerId = null
            });
        }

        if (mediumIds.TryGetValue("Transferencia", out var transferType) && !existing.Contains("Transferencia"))
        {
            seeds.Add(new PaymentMethodsEntity
            {
                PaymentMethodTypeId = transferType,
                DisplayName = "Transferencia",
                CardTypeId = null,
                CardIssuerId = null
            });
        }

        if (mediumIds.TryGetValue("Tarjeta", out var cardType) && !existing.Contains("Tarjeta VISA"))
        {
            seeds.Add(new PaymentMethodsEntity
            {
                PaymentMethodTypeId = cardType,
                DisplayName = "Tarjeta VISA",
                CardTypeId = TryGet(cardTypeIds, "VISA"),
                CardIssuerId = TryGet(issuerIds, "Banco Demo")
            });
        }

        if (seeds.Count == 0)
            return;

        await db.PaymentMethods.AddRangeAsync(seeds, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}

