using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingBookedAt
{
    public DateTime Value { get; }

    private BookingBookedAt(DateTime value) => Value = value;

    public static BookingBookedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de reserva no es valida.", nameof(value));

        return new BookingBookedAt(value);
    }
}
