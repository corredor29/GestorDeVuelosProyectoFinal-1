using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

public sealed class BaggageChargedPrice
{
    public decimal Value { get; }
    private BaggageChargedPrice(decimal value) => Value = value;

    public static BaggageChargedPrice Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El precio cobrado no puede ser negativo.");

        return new BaggageChargedPrice(value);
    }
}