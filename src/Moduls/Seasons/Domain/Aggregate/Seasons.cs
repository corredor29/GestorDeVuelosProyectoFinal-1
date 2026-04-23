using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;

public sealed class Season
{
    public SeasonsId? Id { get; private set; }
    public SeasonsName Name { get; private set; } = null!;
    public SeasonsDescription Description { get; private set; } = null!;
    public SeasonsPriceFactor PriceFactor { get; private set; } = null!;

    private Season() { }

    private Season(
        SeasonsId? id,
        SeasonsName name,
        SeasonsDescription description,
        SeasonsPriceFactor priceFactor)
    {
        Id = id;
        Name = name;
        Description = description;
        PriceFactor = priceFactor;
    }

    public static Season Create(string name, string? description, decimal priceFactor = 1.0000m)
    {
        return new Season(
            id: null,
            name: SeasonsName.Create(name),
            description: SeasonsDescription.Create(description),
            priceFactor: SeasonsPriceFactor.Create(priceFactor));
    }

    public static Season FromPrimitives(int id, string name, string? description, decimal priceFactor)
    {
        return new Season(
            id: SeasonsId.Create(id),
            name: SeasonsName.Create(name),
            description: SeasonsDescription.Create(description),
            priceFactor: SeasonsPriceFactor.Create(priceFactor));
    }

    public void Update(string name, string? description, decimal priceFactor)
    {
        Name = SeasonsName.Create(name);
        Description = SeasonsDescription.Create(description);
        PriceFactor = SeasonsPriceFactor.Create(priceFactor);
    }
}
