using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

public sealed class PaymentMediumTypesId
{
    public int Value { get; }

    private PaymentMediumTypesId(int value) => Value = value;

    public static PaymentMediumTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de medio de pago no es válido", nameof(value));

        return new PaymentMediumTypesId(value);
    }
}
