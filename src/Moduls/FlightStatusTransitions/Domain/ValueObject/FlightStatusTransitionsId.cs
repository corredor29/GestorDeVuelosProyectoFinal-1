using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;

public sealed class FlightStatusTransitionsId
{
    public int Value { get; }

    private FlightStatusTransitionsId(int value) => Value = value;

    public static FlightStatusTransitionsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la transicion de estado no es valido", nameof(value));

        return new FlightStatusTransitionsId(value);
    }
}
