using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed record AddressesPostalCode // <--- Cambiado de PostalCode a AddressesPostalCode
{
    public string? Value { get; }
    public  AddressesPostalCode(string? value) => Value = value;

    public static AddressesPostalCode Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new AddressesPostalCode((string?)null);

        var cleaned = value.Trim();
        
        if (cleaned.Length > 20)
            throw new ArgumentOutOfRangeException(nameof(value), "El código postal es demasiado largo.");

        return new AddressesPostalCode(cleaned);
    }
}