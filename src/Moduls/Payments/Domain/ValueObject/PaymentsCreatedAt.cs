using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

public sealed class PaymentsCreatedAt
{
    public DateTime Value { get; }

    private PaymentsCreatedAt(DateTime value) => Value = value;

    public static PaymentsCreatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de creación de la reserva no es válida", nameof(value));

        // la creación no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de creación de la reserva no puede ser en el pasado", nameof(value));

        return new PaymentsCreatedAt(value);
    }
}
