using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

public sealed class PaymentStatuseId
{
    public int Value { get; }

    private PaymentStatuseId(int value) => Value = value;

    public static PaymentStatuseId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del estado de pago no es válido", nameof(value));

        return new PaymentStatuseId(value);
    }
}
