using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

public sealed class CustomersCreatedAt
{
    public DateTime Value { get; }

    private CustomersCreatedAt(DateTime value) => Value = value;

    public static CustomersCreatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("El campo created_at no puede ser DateTime.MinValue.", nameof(value));

        return new CustomersCreatedAt(value);
    }
}
