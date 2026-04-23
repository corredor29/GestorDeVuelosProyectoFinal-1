using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reservations.Domain.ValueObject;

/// <summary>
/// Identificador de reserva asociado a facturación (mapea a booking_id en persistencia).
/// </summary>
public sealed class ReverseId
{
    public int Value { get; }

    private ReverseId(int value) => Value = value;

    public static ReverseId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de reserva no es válido", nameof(value));

        return new ReverseId(value);
    }
}
