using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

public sealed class PaymentMediumTypesName
{
    public string Value { get; }

    private PaymentMediumTypesName(string value) => Value = value;

    public static PaymentMediumTypesName Create(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("El nombre del tipo de medio de pago no puede superar los 50 caracteres", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de medio de pago no es válido", nameof(value));

        return new PaymentMediumTypesName(value);
    }
}
