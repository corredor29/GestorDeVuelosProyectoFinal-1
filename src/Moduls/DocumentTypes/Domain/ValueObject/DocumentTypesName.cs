using System;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

public sealed class DocumentTypesName
{
    public string Value { get; }
    private DocumentTypesName(string value) => Value = value;

    public static DocumentTypesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de documento no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del tipo de documento no puede tener más de 100 caracteres y menos de 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del tipo de documento no puede contener solo números", nameof(value));

        return new DocumentTypesName(value.Trim());
    }
}