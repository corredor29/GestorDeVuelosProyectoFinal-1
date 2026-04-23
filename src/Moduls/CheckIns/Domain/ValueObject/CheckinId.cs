using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;

public sealed class CheckinsId
{
    public int Value { get; }

    private CheckinsId(int value) => Value = value;

    public static CheckinsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del check-in no es válido.");

        return new CheckinsId(value);
    }
}
