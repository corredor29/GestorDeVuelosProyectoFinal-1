using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

public sealed class PaymentMethodsId
{
    public int Value { get; }

    private PaymentMethodsId(int value) => Value = value;

    public static PaymentMethodsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del medio de pago no es válido", nameof(value));

        return new PaymentMethodsId(value);
    }
}
