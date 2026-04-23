using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

public sealed class RatesId
{
    public int Value { get; }

    private RatesId(int value) => Value = value;

    public static RatesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la tarifa no es valido", nameof(value));

        return new RatesId(value);
    }
}
