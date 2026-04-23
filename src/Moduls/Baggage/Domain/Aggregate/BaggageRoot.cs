using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;

public sealed class BaggageRoot
{
    private BaggageWeightKg _weightKg;
    private BaggageChargedPrice _chargedPrice;

    public BaggageId Id { get; }
    public CheckinsId CheckinId { get; }
    public BaggageTypeId BaggageTypeId { get; }
    public BaggageWeightKg WeightKg => _weightKg;
    public BaggageChargedPrice ChargedPrice => _chargedPrice;

    private BaggageRoot(
        BaggageId id,
        CheckinsId checkinId,
        BaggageTypeId baggageTypeId,
        BaggageWeightKg weightKg,
        BaggageChargedPrice chargedPrice)
    {
        Id            = id;
        CheckinId     = checkinId;
        BaggageTypeId = baggageTypeId;
        _weightKg     = weightKg;
        _chargedPrice = chargedPrice;
    }

    public static BaggageRoot Create(
        int id,
        int checkinId,
        int baggageTypeId,
        decimal weightKg,
        decimal chargedPrice)
    {
        return new BaggageRoot(
            BaggageId.Create(id),
            CheckinsId.Create(checkinId),
            BaggageTypeId.Create(baggageTypeId),
            BaggageWeightKg.Create(weightKg),
            BaggageChargedPrice.Create(chargedPrice)
        );
    }

    public static BaggageRoot CreateNew(
        int checkinId,
        int baggageTypeId,
        decimal weightKg,
        decimal chargedPrice)
    {
        return new BaggageRoot(
            BaggageId.Create(0),
            CheckinsId.Create(checkinId),
            BaggageTypeId.Create(baggageTypeId),
            BaggageWeightKg.Create(weightKg),
            BaggageChargedPrice.Create(chargedPrice)
        );
    }

    public void UpdateWeightKg(decimal value)     => _weightKg     = BaggageWeightKg.Create(value);
    public void UpdateChargedPrice(decimal value) => _chargedPrice = BaggageChargedPrice.Create(value);
}