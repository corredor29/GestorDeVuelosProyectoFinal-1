using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

public sealed class BookingStatusesId
{
    public int Value { get; }

    private BookingStatusesId(int value) => Value = value;

    public static BookingStatusesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la reserva no es válido", nameof(value));

        return new BookingStatusesId(value);
    }
}