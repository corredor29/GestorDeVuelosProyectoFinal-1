using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed class AddressesId
{
    public int Value { get; }
    private AddressesId(int value) => Value = value;
    public static AddressesId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id de la dirección no es válido", nameof(value));
        return new AddressesId(value);
    }
}