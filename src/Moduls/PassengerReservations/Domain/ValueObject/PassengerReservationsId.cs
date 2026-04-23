using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;

public sealed class PassengerReservationId
{
    public int Value { get; }
    private PassengerReservationId(int value) => Value = value;

    public static PassengerReservationId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID de la reserva del pasajero no es válido.");
        return new PassengerReservationId(value);
    }
}