using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;

public sealed record PersonPhonesPhoneNumber
{
    public string Value { get; }
    private PersonPhonesPhoneNumber(string value) => Value = value;

    public static PersonPhonesPhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número telefónico es obligatorio.", nameof(value));

        var trimmed = value.Trim();
        if (!trimmed.All(char.IsDigit))
            throw new ArgumentException("El número telefónico solo puede contener números.", nameof(value));

        if (trimmed.Length > 20)
            throw new ArgumentOutOfRangeException(nameof(value), "El número telefónico no puede superar los 20 caracteres.");

        return new PersonPhonesPhoneNumber(trimmed);
    }
}
