using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;

public sealed class BookingFlightsId
{
    public int Value { get; }

    private BookingFlightsId(int value) => Value = value;

    public static BookingFlightsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de booking_flight no es valido.", nameof(value));

        return new BookingFlightsId(value);
    }
}
