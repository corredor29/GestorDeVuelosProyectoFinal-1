using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

public sealed class PaymentsDate
{
    public DateTime Value { get; }

    private PaymentsDate(DateTime value) => Value = value;

    public static PaymentsDate Create(DateTime value)
    // verifica que la fecha no venga vacia
    {   
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de reserva no es válida", nameof(value));

        // la reserva no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de reserva no puede ser en el pasado", nameof(value));

        return new PaymentsDate(value);
    }
}
