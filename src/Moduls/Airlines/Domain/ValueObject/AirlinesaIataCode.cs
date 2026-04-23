using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;

public sealed record AirlinesIataCode
{
    public string Value { get; }

    private AirlinesIataCode(string value) => Value = value;

    public static AirlinesIataCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código IATA es obligatorio.");

        var code = value.Trim().ToUpperInvariant();

        if (!Regex.IsMatch(code, "^[A-Z]{3}$"))
            throw new ArgumentException("El código IATA debe tener exactamente 3 letras.");

        return new AirlinesIataCode(code);
    }
}
