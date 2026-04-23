using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

public sealed class RatesBasePrice
{
    public decimal Value { get; }

    private RatesBasePrice(decimal value) => Value = value;

    public static RatesBasePrice Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El precio base de la tarifa no es válido", nameof(value));

        return new RatesBasePrice(value);
    }
}
