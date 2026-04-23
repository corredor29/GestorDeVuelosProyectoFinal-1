using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;

public sealed class AircraftManufacturersName
{
    public string Value { get; }
    private AircraftManufacturersName(string value) => Value = value;

    public static AircraftManufacturersName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del fabricante de aviones no puede estar vacío", nameof(value));

        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del fabricante de aviones no puede superar los 100 caracteres.");

        return new AircraftManufacturersName(value.Trim());
    }

    public override string ToString() => Value;
}
