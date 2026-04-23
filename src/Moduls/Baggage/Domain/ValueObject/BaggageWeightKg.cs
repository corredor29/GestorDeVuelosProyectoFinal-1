using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

public sealed class BaggageWeightKg
{
    public decimal Value { get; }
    private BaggageWeightKg(decimal value) => Value = value;

    public static BaggageWeightKg Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El peso del equipaje no puede ser negativo.");

        if (value > 999.99m)
            throw new ArgumentException("El peso del equipaje no puede superar 999.99 kg.");

        return new BaggageWeightKg(value);
    }
}