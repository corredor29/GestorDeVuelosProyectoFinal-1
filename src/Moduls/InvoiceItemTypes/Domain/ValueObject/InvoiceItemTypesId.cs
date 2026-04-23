using System;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

public sealed class InvoiceItemTypesId
{
    public int Value { get; }
    private InvoiceItemTypesId(int value) => Value = value;
    public static InvoiceItemTypesId Create(int value)
    {
        if (value <= 0) throw new ArgumentException("El ID del tipo de artículo no es válido.");
        return new InvoiceItemTypesId(value);
    }
}
