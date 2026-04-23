using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

public sealed class PermissionsDescription
{
    public string? Value { get; }
    private PermissionsDescription(string? value) => Value = value;

    public static PermissionsDescription Create(string? value)
    {
        if (value != null && value.Length > 200)
            throw new ArgumentException("La descripción del permiso no puede exceder los 200 caracteres.");

        return new PermissionsDescription(value?.Trim());
    }
}