using System;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;

public sealed class FlightRolesId
{
    public int Value { get; }
    private FlightRolesId(int value) => Value = value;
    public static FlightRolesId Create(int value)
    {
        if (value <= 0) throw new ArgumentException("The flight role ID is invalid.");
        return new FlightRolesId(value);
    }
}
