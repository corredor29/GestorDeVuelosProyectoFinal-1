using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;

public sealed class CabinSeatLetters
{
    public string Value { get; }
    private CabinSeatLetters(string value) => Value = value;

    public static CabinSeatLetters Create(string value, int expectedCount)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Las letras de los asientos son obligatorias.");

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length != expectedCount)
            throw new ArgumentException($"La cantidad de letras ({normalized.Length}) no coincide con los asientos por fila ({expectedCount}).");

        if (normalized.Any(c => !char.IsLetter(c)))
            throw new ArgumentException("Las letras de los asientos solo pueden contener caracteres alfabéticos.");

        if (normalized.Distinct().Count() != normalized.Length)
            throw new ArgumentException("Las letras de los asientos no pueden repetirse.");

        return new CabinSeatLetters(normalized);
    }
}
