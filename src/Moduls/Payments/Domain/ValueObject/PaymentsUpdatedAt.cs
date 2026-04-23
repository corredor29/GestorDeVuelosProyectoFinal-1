using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

public sealed class PaymentsUpdatedAt
{
    public DateTime Value { get; }

    private PaymentsUpdatedAt(DateTime value) => Value = value;

    public static PaymentsUpdatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de actualización de la reserva no es válida", nameof(value));

        // la actualización no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de actualización de la reserva no puede ser en el pasado", nameof(value));

        return new PaymentsUpdatedAt(value);
    }
}
