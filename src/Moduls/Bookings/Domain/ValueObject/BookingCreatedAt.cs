using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingCreatedAt
{
    public DateTime Value { get; }

    private BookingCreatedAt(DateTime value) => Value = value;

    public static BookingCreatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de creacion no es valida.", nameof(value));

        return new BookingCreatedAt(value);
    }
}
