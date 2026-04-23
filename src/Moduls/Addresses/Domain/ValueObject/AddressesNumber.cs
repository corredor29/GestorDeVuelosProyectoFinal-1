using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed record AddressesNumber
{
    public string? Value { get; }

    public AddressesNumber(string? value) => Value = value;

    public static AddressesNumber Create(string? value)
    {
        // Si el número es nulo o espacios, lo normalizamos a null (ya que es opcional en la BD)
        if (string.IsNullOrWhiteSpace(value))
        {
            return new AddressesNumber((string?)null);
        }

        var trimmedValue = value.Trim();

        // Validación de longitud según tu VARCHAR(20)
        if (trimmedValue.Length > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El número de dirección no puede superar los 20 caracteres.");
        }

        return new AddressesNumber(trimmedValue);
    }

    public override string ToString() => Value ?? string.Empty;
}