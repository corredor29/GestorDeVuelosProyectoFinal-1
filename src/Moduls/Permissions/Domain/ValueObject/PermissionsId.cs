using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

public sealed class PermissionsId
{
    public int Value { get; }
    private PermissionsId(int value) => Value = value;

    public static PermissionsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del permiso no es válido.");
        return new PermissionsId(value);
    }
}