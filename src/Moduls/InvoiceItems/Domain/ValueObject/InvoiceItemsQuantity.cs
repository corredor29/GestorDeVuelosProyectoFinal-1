namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

public sealed class InvoiceItemsQuantity
{
    public int Value { get; }
    private InvoiceItemsQuantity(int value) => Value = value;

    public static InvoiceItemsQuantity Create(int value)
    {
        if (value < 1)
            throw new ArgumentException("La cantidad mínima de un ítem debe ser 1.");
            
        return new InvoiceItemsQuantity(value);
    }
}