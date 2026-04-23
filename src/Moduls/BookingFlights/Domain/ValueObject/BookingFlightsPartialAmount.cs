using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;

public sealed class BookingFlightsPartialAmount
{
    public decimal Value { get; }

    private BookingFlightsPartialAmount(decimal value) => Value = value;

    public static BookingFlightsPartialAmount Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El partial_amount no puede ser negativo.", nameof(value));

        return new BookingFlightsPartialAmount(decimal.Round(value, 2, MidpointRounding.AwayFromZero));
    }
}
