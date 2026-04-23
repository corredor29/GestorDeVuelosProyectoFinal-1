using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

public sealed class CountryId
{
    public int Value { get; }

    private CountryId(int value) => Value = value;

    public static CountryId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del país no es válido", nameof(value));

        return new CountryId(value);
    }
}