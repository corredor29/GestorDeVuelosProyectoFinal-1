using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

public sealed record PhoneCodesCountryCode
{
    public string Value { get; }
    private PhoneCodesCountryCode(string value) => Value = value;

    public static PhoneCodesCountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código de país es obligatorio.");

        var raw = value.Trim();
        var digits = raw.StartsWith("+", StringComparison.Ordinal) ? raw[1..] : raw;

        if (string.IsNullOrWhiteSpace(digits))
            throw new ArgumentException("El código de país es obligatorio.");

        if (!digits.All(char.IsDigit))
            throw new ArgumentException("El código de país solo puede contener números.");

        if (digits.Length > 4)
            throw new ArgumentOutOfRangeException(nameof(value), "El código de país no puede superar los 4 dígitos.");

        var formatted = $"+{digits}";

        return new PhoneCodesCountryCode(formatted);
    }
}
