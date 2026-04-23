namespace GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelFuelConsumption
{
    public decimal? Value { get; }
    private AircraftModelFuelConsumption(decimal? value) => Value = value;
    public static AircraftModelFuelConsumption Create(decimal? value)
    {
        if (value.HasValue && value < 0)
            throw new ArgumentException("El consumo de combustible no puede ser negativo.", nameof(value));
        return new AircraftModelFuelConsumption(value);
    }
    public override string ToString() => Value?.ToString() ?? "N/A";
}