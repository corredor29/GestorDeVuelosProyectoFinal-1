using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

public sealed class PaymentMethodsDisplayName
{
    public string Value { get; }

    private PaymentMethodsDisplayName(string value) => Value = value;

    public static PaymentMethodsDisplayName Create(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("El nombre del medio de pago no puede superar los 50 caracteres", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del medio de pago no es válido", nameof(value));

        return new PaymentMethodsDisplayName(value);
    }
}
