using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed record AddressesNameVia
{
    public string Value { get; }

    private AddressesNameVia(string value) => Value = value;

    public static AddressesNameVia Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la calle o vía es obligatorio.");

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre de la vía no puede superar los 100 caracteres.");

        return new AddressesNameVia(trimmedValue);
    }
}