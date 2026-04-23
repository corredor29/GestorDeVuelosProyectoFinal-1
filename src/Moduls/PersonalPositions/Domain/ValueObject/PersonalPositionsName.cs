using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

public sealed class PersonalPositionsName
{
    public string Value { get; }
    private PersonalPositionsName(string value) => Value = value;

    public static PersonalPositionsName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del personal no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del personal no puede tener más de 100 caracteres y menos de 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del personal no puede contener solo números", nameof(value));

        return new PersonalPositionsName(value.Trim());
    }
}   