using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

public sealed class BaggageTypeName
{
    public string Value { get; }
    private BaggageTypeName(string value) => Value = value;

    public static BaggageTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de equipaje es obligatorio.");

        if (value.Length < 2 || value.Length > 50)
            throw new ArgumentException("El nombre del tipo de equipaje debe tener entre 2 y 50 caracteres.");

        return new BaggageTypeName(value.Trim());
    }
}