using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

public sealed class BaggageTypeId
{
    public int Value { get; }
    private BaggageTypeId(int value) => Value = value;

    public static BaggageTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de equipaje no es válido.");
        return new BaggageTypeId(value);
    }
}