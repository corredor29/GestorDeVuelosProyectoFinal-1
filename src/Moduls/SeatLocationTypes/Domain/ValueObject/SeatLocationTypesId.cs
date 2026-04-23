using System;

namespace GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

public sealed class SeatLocationTypesId
{
    public int Value { get; }
    private SeatLocationTypesId(int value) => Value = value;
    public static SeatLocationTypesId Create(int value)
    {
        if (value <= 0) throw new ArgumentException("The flight role ID is invalid.");
        return new SeatLocationTypesId(value);
    }
}