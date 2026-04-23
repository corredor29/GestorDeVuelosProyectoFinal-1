using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleLastNames
{
    public string Value { get; }
    private PeopleLastNames(string value) => Value = value;
    public static PeopleLastNames Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Los apellidos son obligatorios.");
        return new PeopleLastNames(value.Trim());
    }
}