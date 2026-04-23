using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

public sealed class BookingStatusesName
{
    public string Value { get; }

    private BookingStatusesName(string value) => Value = value;

    public static BookingStatusesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la reserva no es válido", nameof(value));

        if (value.Length > 50 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre de la reserva no puede superar los 50 caracteres");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre de la reserva no puede contener solo números", nameof(value));

        return new BookingStatusesName(value.Trim());
    }
}
