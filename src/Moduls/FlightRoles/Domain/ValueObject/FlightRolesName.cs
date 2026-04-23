using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;

public sealed class FlightRolesName
{
    public string Value { get; }
    private FlightRolesName(string value) => Value = value;

    public static FlightRolesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The flight role name cannot be empty.", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "The flight role name cannot be longer than 100 characters and shorter than 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("The flight role name cannot contain only numbers", nameof(value));

        return new FlightRolesName(value.Trim());
    }
}