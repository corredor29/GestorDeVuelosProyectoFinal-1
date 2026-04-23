using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

public sealed class PassengersId
{
    public int Value { get; }

    private PassengersId(int value) => Value = value;

    public static PassengersId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del pasajero no es válido", nameof(value));

        return new PassengersId(value);
    }
}
