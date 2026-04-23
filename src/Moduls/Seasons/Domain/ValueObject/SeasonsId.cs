using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

public sealed class SeasonsId
{
    public int Value { get; }

    private SeasonsId(int value) => Value = value;

    public static SeasonsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la temporada no es valido", nameof(value));

        return new SeasonsId(value);
    }
}
