namespace GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelCruiseAltitude
{
    public int? Value { get; }
    private AircraftModelCruiseAltitude(int? value) => Value = value;
    public static AircraftModelCruiseAltitude Create(int? value)
    {
        if (value.HasValue && value < 0)
            throw new ArgumentException("La altitud de crucero no puede ser negativa.", nameof(value));
        return new AircraftModelCruiseAltitude(value);
    }
    public override string ToString() => Value?.ToString() ?? "N/A";
}