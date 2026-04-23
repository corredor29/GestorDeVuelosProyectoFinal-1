using System;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;

public sealed class CabinSeatsPerRow
{
    public int Value { get; }
    private CabinSeatsPerRow(int value) => Value = value;
    public static CabinSeatsPerRow Create(int value)
    {
        if (value <= 0 || value > 20) // Un avión comercial rara vez tiene más de 10-12
            throw new ArgumentException("La cantidad de asientos por fila debe estar entre 1 y 20.");
        return new CabinSeatsPerRow(value);
    }
}