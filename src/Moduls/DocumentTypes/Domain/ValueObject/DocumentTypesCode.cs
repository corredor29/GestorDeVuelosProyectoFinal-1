using System;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

public sealed record DocumentTypesCode
{
    public string Value { get; }
    private DocumentTypesCode(string value) => Value = value;

    public static DocumentTypesCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código del documento es obligatorio.");

        var formatted = value.Trim().ToUpper();

        if (formatted.Length > 10)
            throw new ArgumentOutOfRangeException(nameof(value), "El código no puede superar los 10 caracteres.");

        return new DocumentTypesCode(formatted);
    }
}