using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

public sealed class PassengersTypeId
{
    public int Value { get; }
    private PassengersTypeId(int value) => Value = value;

    public static PassengersTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de tipo de pasajero no es válido", nameof(value));

        return new PassengersTypeId(value);
    }
}