using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

public sealed class InvoicesId
{
    public int Value { get; }
    private InvoicesId(int value) => Value = value;

    public static InvoicesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la factura no es válido.");
        return new InvoicesId(value);
    }
}
