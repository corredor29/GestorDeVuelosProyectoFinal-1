using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

public sealed record AirportsIataCode
{
    public string Value { get; }

    private AirportsIataCode(string value) => Value = value;

    public static AirportsIataCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código IATA del aeropuerto es obligatorio.");

        var code = value.Trim().ToUpperInvariant();
        if (!Regex.IsMatch(code, "^[A-Z]{3}$"))
            throw new ArgumentException("El código IATA debe tener exactamente 3 letras.");

        return new AirportsIataCode(code);
    }
}
