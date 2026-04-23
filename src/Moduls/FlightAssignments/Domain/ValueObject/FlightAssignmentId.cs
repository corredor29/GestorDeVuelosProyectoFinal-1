using System;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;


public sealed class FlightAssignmentId 
{
    public int Value { get; }
    private FlightAssignmentId(int value) => Value = value;
    public static FlightAssignmentId Create(int value)
    {
        if (value <= 0) throw new ArgumentException("El ID de asignación de vuelo no es válido.");
        return new FlightAssignmentId(value);
    }
}
