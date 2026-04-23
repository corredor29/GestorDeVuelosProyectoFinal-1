using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

public sealed class PaymentsId
{
    public int Value { get; }    
    private PaymentsId(int value) => Value = value;
    public static PaymentsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la reserva no es válido", nameof(value));

        return new PaymentsId(value);
    }
}
