using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PersonUpdatedAt
{
    public DateTime Value { get; }
    private PersonUpdatedAt(DateTime value) => Value = value;

    public static PersonUpdatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de actualización no es válida.", nameof(value));

        return new PersonUpdatedAt(value);
    }
}
