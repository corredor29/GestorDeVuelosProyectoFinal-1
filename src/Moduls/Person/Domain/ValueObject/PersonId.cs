using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleId
{
    public int Value { get; }
    private PeopleId(int value) => Value = value;

    public static PeopleId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la persona no es válido.", nameof(value));

        return new PeopleId(value);
    }
}
