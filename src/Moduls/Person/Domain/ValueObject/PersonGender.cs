using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleGender
{
    public char? Value { get; }
    private static readonly char[] ValidGenders = { 'M', 'F', 'N' };

    private PeopleGender(char? value) => Value = value;

    public static PeopleGender Create(char? value)
    {
        if (value.HasValue)
        {
            char upperGender = char.ToUpper(value.Value);
            if (!ValidGenders.Contains(upperGender))
                throw new ArgumentException("El género debe ser M, F o N.");
            
            return new PeopleGender(upperGender);
        }

        return new PeopleGender((char?)null);
    }
}