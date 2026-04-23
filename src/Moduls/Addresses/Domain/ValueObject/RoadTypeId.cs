using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;

public sealed record RoadTypeId
{
    public int Value { get; }
    private RoadTypeId(int value) => Value = value;

    public static RoadTypeId Create(int value)
    {
        // Validamos que sea un ID de catálogo válido (generalmente > 0)
        if (value <= 0)
            throw new ArgumentException("Debe seleccionar un tipo de vía válido.");

        return new RoadTypeId(value);
    }
}