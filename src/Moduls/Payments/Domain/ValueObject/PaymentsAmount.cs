using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

public sealed class PaymentsAmount
{
    public decimal Value { get; }

    private PaymentsAmount(decimal value) => Value = value;

    public static PaymentsAmount Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El monto de la reserva no es válido", nameof(value));

        return new PaymentsAmount(value);
    }
}
