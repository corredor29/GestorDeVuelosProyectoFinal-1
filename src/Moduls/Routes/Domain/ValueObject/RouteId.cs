using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

public sealed class RouteId
{
    public int Value { get; }
    private RouteId(int value) => Value = value;
    public static RouteId Create(int value)
    {
        if (value <= 0)
        throw new ArgumentException("El ID de la ruta no es válido.");
        return new RouteId(value);
    }
}