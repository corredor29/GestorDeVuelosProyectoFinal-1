namespace GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelName
{
    public string Value { get; }
    private AircraftModelName(string value) => Value = value;
    public static AircraftModelName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("El nombre del modelo es obligatorio.", nameof(value));
        if (value.Trim().Length > 100)
            throw new ArgumentException("El nombre no puede superar 100 caracteres.", nameof(value));

        return new AircraftModelName(value.Trim());
    }
    public override string ToString() => Value;
}