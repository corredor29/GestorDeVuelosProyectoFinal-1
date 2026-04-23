using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

public sealed class RegionName
{
    public string Value { get; }
    private RegionName(string value) => Value = value;

    public static RegionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la región no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre de la región no puede tener más de 100 caracteres y menos de 2.");

        var trimmed = value.Trim();
        if (!trimmed.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("El nombre de la región solo puede contener letras y espacios", nameof(value));

        return new RegionName(trimmed);
    }
}
