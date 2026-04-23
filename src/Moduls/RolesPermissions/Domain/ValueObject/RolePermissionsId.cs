using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;

public sealed class RolePermissionsId
{
    public int Value { get; }
    private RolePermissionsId(int value) => Value = value;

    public static RolePermissionsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la relación rol-permiso no es válido.");
        return new RolePermissionsId(value);
    }
}