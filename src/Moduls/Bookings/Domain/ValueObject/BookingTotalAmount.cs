using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingTotalAmount
{
    public decimal Value { get; }

    private BookingTotalAmount(decimal value) => Value = value;

    public static BookingTotalAmount Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El total de la reserva no puede ser negativo.", nameof(value));

        return new BookingTotalAmount(decimal.Round(value, 2, MidpointRounding.AwayFromZero));
    }
}
