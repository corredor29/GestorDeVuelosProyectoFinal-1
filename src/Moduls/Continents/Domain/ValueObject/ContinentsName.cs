using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

public sealed record ContinentName // Sin la 's' para que coincida con el Agregado
{
    public string Value { get; }
    public ContinentName(string value) => Value = value;

    public static ContinentName Create(string value) // PascalCase
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del continente no puede estar vacío", nameof(value));

        if (value.Length > 50 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre debe tener entre 2 y 50 caracteres.");

        if (value.Any(char.IsDigit))
            throw new ArgumentException("El nombre del continente no puede contener números", nameof(value));

        return new ContinentName(value.Trim());
    }
}
