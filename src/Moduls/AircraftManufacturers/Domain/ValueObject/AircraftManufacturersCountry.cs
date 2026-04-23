using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;

public sealed class AircraftManufacturersCountry
{
    public string Value { get; }
    private AircraftManufacturersCountry(string value) => Value = value;

    public static AircraftManufacturersCountry Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El país del fabricante de aviones no puede estar vacío", nameof(value));

        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "El país del fabricante de aviones no puede superar los 100 caracteres.");

        return new AircraftManufacturersCountry(value.Trim());
    }

    public override string ToString() => Value;
}
