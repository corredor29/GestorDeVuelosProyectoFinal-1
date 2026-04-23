using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsId
{
    public int Value { get; }

    private FlightsId(int value) => Value = value;

    public static FlightsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del vuelo no es válido", nameof(value));

        return new FlightsId(value);
    }
}
