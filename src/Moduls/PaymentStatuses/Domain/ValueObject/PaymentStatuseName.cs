using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

public sealed class PaymentStatuseName
{
    public string Value { get; }

    private PaymentStatuseName(string value) => Value = value;

    public static PaymentStatuseName Create(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("El nombre del estado de pago no puede superar los 50 caracteres", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del estado de pago no es válido", nameof(value));

        return new PaymentStatuseName(value);
    }
}
