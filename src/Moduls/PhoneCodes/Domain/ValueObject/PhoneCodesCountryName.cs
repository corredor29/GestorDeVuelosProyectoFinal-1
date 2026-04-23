using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

public sealed record PhoneCodesCountryName
{
    public string Value { get; }
    private PhoneCodesCountryName(string value) => Value = value;

    public static PhoneCodesCountryName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del país es obligatorio.");

        var trimmed = value.Trim();

        if (trimmed.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del país no puede superar los 100 caracteres.");

        return new PhoneCodesCountryName(trimmed);
    }
}