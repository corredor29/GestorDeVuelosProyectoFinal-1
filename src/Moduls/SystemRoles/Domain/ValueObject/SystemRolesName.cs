using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

public sealed class RolesName
{
    public string Value { get; }
    private RolesName(string value) => Value = value;

    public static RolesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del rol es obligatorio.");

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("El nombre del rol debe tener entre 3 y 50 caracteres.");

        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del rol no puede contener solo números.");

        return new RolesName(value.Trim());
    }
}