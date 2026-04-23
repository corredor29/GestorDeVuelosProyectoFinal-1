using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

public sealed class SeatLocationTypesName
{
    public string Value { get; }
    private SeatLocationTypesName(string value) => Value = value;

    public static SeatLocationTypesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The seat location type name cannot be empty.", nameof(value));

        if (value.Length > 50 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "The name of the seating location type cannot be longer than 50 characters and shorter than 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("The seat location type name cannot contain only numbers", nameof(value));

        return new SeatLocationTypesName(value.Trim());
    }
}