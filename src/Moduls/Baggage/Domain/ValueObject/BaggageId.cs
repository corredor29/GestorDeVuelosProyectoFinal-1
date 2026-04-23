using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

public sealed class BaggageId
{
    public int Value { get; }
    private BaggageId(int value) => Value = value;

    public static BaggageId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del equipaje no es válido.");
        return new BaggageId(value);
    }
}
