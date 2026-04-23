using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

public sealed class ContinentsId
{
    public int Value { get; }
    public ContinentsId(int value) => Value = value;
    public static ContinentsId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del continente no es válido", nameof(value));
        return new ContinentsId(value);
    }
}