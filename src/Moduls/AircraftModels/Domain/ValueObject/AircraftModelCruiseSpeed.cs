namespace GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelCruiseSpeed
{
    public int? Value { get; }
    private AircraftModelCruiseSpeed(int? value) => Value = value;
    public static AircraftModelCruiseSpeed Create(int? value)
    {
        if (value.HasValue && value < 0)
            throw new ArgumentException("La velocidad de crucero no puede ser negativa.", nameof(value));
        return new AircraftModelCruiseSpeed(value);
    }
    public override string ToString() => Value?.ToString() ?? "N/A";
}