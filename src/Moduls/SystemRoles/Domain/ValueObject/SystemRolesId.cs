using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

public sealed class RolesId
{
    public int Value { get; }
    private RolesId(int value) => Value = value;

    public static RolesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del rol no es válido.");
        return new RolesId(value);
    }
}