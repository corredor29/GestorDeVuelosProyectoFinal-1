namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightCode
{
    public const int MaxLength = 10;

    public string Value { get; }

    private FlightCode(string value) => Value = value;

    public static FlightCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El codigo de vuelo es obligatorio.");

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"El codigo de vuelo no puede superar {MaxLength} caracteres.");

        return new FlightCode(trimmed);
    }
}
