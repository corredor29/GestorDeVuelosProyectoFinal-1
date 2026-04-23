using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

public sealed class AirportsIcaoCode
{
    public string? Value { get; }

    private AirportsIcaoCode(string? value) => Value = value;

    public static AirportsIcaoCode Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new AirportsIcaoCode(null);

        var code = value.Trim().ToUpperInvariant();
        if (!Regex.IsMatch(code, "^[A-Z]{4}$"))
            throw new ArgumentException("El código ICAO debe tener exactamente 4 letras.");

        return new AirportsIcaoCode(code);
    }
}
