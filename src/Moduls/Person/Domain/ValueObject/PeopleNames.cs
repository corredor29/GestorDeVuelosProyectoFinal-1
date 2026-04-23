using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleNames
{
    public string Value { get; }
    private PeopleNames(string value) => Value = value;
    public static PeopleNames Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Los nombres son obligatorios.");
        return new PeopleNames(value.Trim());
    }
}