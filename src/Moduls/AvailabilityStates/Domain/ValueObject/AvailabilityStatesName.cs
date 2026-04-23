using System;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;

public sealed class AvailabilityStatesName
{
    public string Value { get; }
    private AvailabilityStatesName(string value) => Value = value;

    public static AvailabilityStatesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de disponibilidad no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del estado de disponibilidad no puede tener más de 100 caracteres y menos de 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del estado de disponibilidad no puede contener solo números", nameof(value));

        return new AvailabilityStatesName(value.Trim());
    }
}