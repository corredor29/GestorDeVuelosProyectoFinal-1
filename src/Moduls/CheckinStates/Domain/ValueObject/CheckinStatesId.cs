using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

public sealed class CheckinStatesId
{
    public int Value { get; }

    private CheckinStatesId(int value) => Value = value;

    public static CheckinStatesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del estado de check-in no es válido", nameof(value));

        return new CheckinStatesId(value);
    }
}