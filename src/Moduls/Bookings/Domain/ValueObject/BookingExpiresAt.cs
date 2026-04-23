using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

public sealed class BookingExpiresAt
{
    public DateTime? Value { get; }

    private BookingExpiresAt(DateTime? value) => Value = value;

    public static BookingExpiresAt Create(DateTime? value)
    {
        if (value.HasValue && value.Value == DateTime.MinValue)
            throw new ArgumentException("La fecha de expiracion no es valida.", nameof(value));

        return new BookingExpiresAt(value);
    }
}
