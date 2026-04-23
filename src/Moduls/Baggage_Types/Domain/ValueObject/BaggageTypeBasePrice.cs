using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

public sealed class BaggageTypeBasePrice
{
    public decimal Value { get; }
    private BaggageTypeBasePrice(decimal value) => Value = value;

    public static BaggageTypeBasePrice Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El precio base no puede ser negativo.");

        return new BaggageTypeBasePrice(value);
    }
}
