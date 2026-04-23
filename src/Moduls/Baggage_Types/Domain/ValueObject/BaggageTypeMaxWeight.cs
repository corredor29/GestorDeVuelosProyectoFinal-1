using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

public sealed class BaggageTypeMaxWeight
{
    public decimal Value { get; }
    private BaggageTypeMaxWeight(decimal value) => Value = value;

    public static BaggageTypeMaxWeight Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("El peso máximo debe ser mayor a 0.");

        if (value > 999.99m)
            throw new ArgumentException("El peso máximo no puede superar 999.99 kg.");

        return new BaggageTypeMaxWeight(value);
    }
}