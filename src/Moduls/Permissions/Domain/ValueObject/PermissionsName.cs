using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

public sealed class PermissionsName
{
    public string Value { get; }
    private PermissionsName(string value) => Value = value;

    public static PermissionsName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del permiso es obligatorio.");

        if (value.Length < 4 || value.Length > 100)
            throw new ArgumentException("El nombre del permiso debe tener entre 4 y 100 caracteres.");

        // Opcional: Podrías forzar mayúsculas si quieres mantener el estándar de tu comentario SQL
        return new PermissionsName(value.Trim().ToUpper());
    }
}