using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

public sealed class SeasonsName
{
    public string Value { get; }

    private SeasonsName(string value) => Value = value;

    public static SeasonsName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la temporada no puede estar vacio", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length > 50 || normalized.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre de la temporada no puede tener mas de 50 caracteres ni menos de 2.");

        if (normalized.All(char.IsDigit))
            throw new ArgumentException("El nombre de la temporada no puede contener solo numeros", nameof(value));

        return new SeasonsName(normalized);
    }
}
