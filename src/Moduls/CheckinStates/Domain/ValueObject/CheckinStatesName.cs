using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

public sealed class CheckinStatesName
{
    public string Value { get; }
    private CheckinStatesName(string value) => Value = value;

    public static CheckinStatesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de check-in no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del estado de check-in no puede tener más de 100 caracteres y menos de 2.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del estado de check-in no puede contener solo números", nameof(value));

        return new CheckinStatesName(value.Trim());
    }
}