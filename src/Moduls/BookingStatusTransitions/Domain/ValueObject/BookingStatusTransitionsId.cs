using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;

public sealed class BookingStatusTransitionsId
{
    public int Value { get; }

    private BookingStatusTransitionsId(int value) => Value = value;

    public static BookingStatusTransitionsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la transicion de estado de reserva no es valido", nameof(value));

        return new BookingStatusTransitionsId(value);
    }
}
