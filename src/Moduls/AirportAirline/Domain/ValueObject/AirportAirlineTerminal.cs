namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;

public sealed class AirportAirlineTerminal
{
    public string? Value { get; }

    private AirportAirlineTerminal(string? value) => Value = value;

    public static AirportAirlineTerminal Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new AirportAirlineTerminal(null);

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new ArgumentOutOfRangeException(nameof(value), "La terminal no puede superar los 20 caracteres.");

        return new AirportAirlineTerminal(normalized);
    }
}
