using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;

public sealed record PersonPhonesId
{
    public int Value { get; }
    private PersonPhonesId(int value) => Value = value;

    public static PersonPhonesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del teléfono de persona no es válido.", nameof(value));

        return new PersonPhonesId(value);
    }
}
