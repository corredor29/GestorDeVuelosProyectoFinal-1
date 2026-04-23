using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

public sealed class InvoiceItemsId
{
    public int Value { get; }
    private InvoiceItemsId(int value) => Value = value;

    public static InvoiceItemsId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del ítem de factura no es válido.");
        return new InvoiceItemsId(value);
    }
}
