namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;

public sealed class FlightStatusName
{
    public const int MaxLength = 50;

    public string Value { get; }

    private FlightStatusName(string value) => Value = value;

    public static FlightStatusName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de vuelo es obligatorio.");

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"El nombre no puede superar {MaxLength} caracteres.");

        return new FlightStatusName(trimmed);
    }

    public override string ToString() => Value;
}
