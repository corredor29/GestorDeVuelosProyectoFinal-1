using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;

public sealed class BaggageType
{
    public BaggageTypeId Id { get; private set; } = null!;
    public BaggageTypeName Name { get; private set; } = null!;
    public BaggageTypeMaxWeight MaxWeightKg { get; private set; } = null!;
    public BaggageTypeBasePrice BasePrice { get; private set; } = null!;

    private BaggageType() { }

    public static BaggageType Create(
        int id,
        string name,
        decimal maxWeightKg,
        decimal basePrice)
    {
        return new BaggageType
        {
            Id = BaggageTypeId.Create(id),
            Name = BaggageTypeName.Create(name),
            MaxWeightKg = BaggageTypeMaxWeight.Create(maxWeightKg),
            BasePrice = BaggageTypeBasePrice.Create(basePrice)
        };
    }

    internal void SetId(int id) => Id = BaggageTypeId.Create(id);

    public void UpdateName(string newName) => Name = BaggageTypeName.Create(newName);
    public void UpdateMaxWeight(decimal newWeight) => MaxWeightKg = BaggageTypeMaxWeight.Create(newWeight);
    public void UpdateBasePrice(decimal newPrice) => BasePrice = BaggageTypeBasePrice.Create(newPrice);
}