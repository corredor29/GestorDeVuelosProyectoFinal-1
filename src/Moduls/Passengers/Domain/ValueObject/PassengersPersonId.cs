using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

public class PassengersPersonId
{
    public int Value { get; }
    private PassengersPersonId(int value) => Value = value;

    public static PassengersPersonId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de persona no es válido", nameof(value));

        return new PassengersPersonId(value);
    }
}