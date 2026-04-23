using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingId
{
    public int Value { get; }

    private BookingId(int value) => Value = value;

    public static BookingId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la reserva no es valido.", nameof(value));

        return new BookingId(value);
    }
}
