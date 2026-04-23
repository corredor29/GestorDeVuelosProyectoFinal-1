using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

public sealed class CustomersId
{
    public int Value { get; }

    private CustomersId(int value) => Value = value;

    public static CustomersId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del cliente no es valido.", nameof(value));

        return new CustomersId(value);
    }

    public override string ToString() => Value.ToString();
}
