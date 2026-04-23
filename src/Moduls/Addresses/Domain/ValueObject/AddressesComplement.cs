using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed record AddressesComplement
{
    public string? Value { get; }

    public AddressesComplement(string? value) => Value = value;

    public static AddressesComplement Create(string? value)
    {
        // Como es NULL en la base de datos, si viene vacío lo normalizamos a null
        if (string.IsNullOrWhiteSpace(value))
        {
            return new AddressesComplement((string?)null);
        }

        var trimmedValue = value.Trim();

        // Validación de longitud según tu esquema VARCHAR(100)
        if (trimmedValue.Length > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El complemento de la dirección no puede superar los 100 caracteres.");
        }

        return new AddressesComplement(trimmedValue);
    }

    // Método para facilitar la visualización o el guardado
    public override string ToString() => Value ?? string.Empty;
}