using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleDocumentNumber
{
    public string Value { get; }
    private PeopleDocumentNumber(string value) => Value = value;

    public static PeopleDocumentNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de documento es obligatorio.");

        var trimmed = value.Trim();
        if (trimmed.Length > 30)
            throw new ArgumentOutOfRangeException(nameof(value), "El documento no puede superar los 30 caracteres.");

        if (!trimmed.All(char.IsDigit))
            throw new ArgumentException("El número de documento solo puede contener dígitos.");

        return new PeopleDocumentNumber(trimmed);
    }
}
