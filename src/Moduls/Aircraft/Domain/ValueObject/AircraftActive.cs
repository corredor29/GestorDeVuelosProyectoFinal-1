using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;

public sealed class AircraftActive
{
    public bool Value { get; }
    private AircraftActive(bool value)
    {
        Value = value;
    }

    public static AircraftActive Create(bool value)
    {
        return new AircraftActive(value);
    }
    public override string ToString() => Value.ToString();
}
