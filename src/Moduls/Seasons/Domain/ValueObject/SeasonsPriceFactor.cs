using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

public sealed class SeasonsPriceFactor
{
    public decimal Value { get; }

    private SeasonsPriceFactor(decimal value) => Value = value;

    public static SeasonsPriceFactor Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("El factor de precio debe ser un numero positivo mayor a cero.");

        if (decimal.Round(value, 4) != value)
            throw new ArgumentException("El factor de precio solo permite hasta 4 decimales.");

        return new SeasonsPriceFactor(value);
    }
}
