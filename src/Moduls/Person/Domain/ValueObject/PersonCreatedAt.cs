using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PersonCreatedAt
{
    public DateTime Value { get; }
    private PersonCreatedAt(DateTime value) => Value = value;

    public static PersonCreatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de creación no es válida.", nameof(value));

        return new PersonCreatedAt(value);
    }
}
