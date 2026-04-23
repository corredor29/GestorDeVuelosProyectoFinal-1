using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingUpdatedAt
{
    public DateTime Value { get; }

    private BookingUpdatedAt(DateTime value) => Value = value;

    public static BookingUpdatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de actualizacion no es valida.", nameof(value));

        return new BookingUpdatedAt(value);
    }
}
