namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

public sealed class InvoiceItemsPrice
{
    public decimal Value { get; }
    private InvoiceItemsPrice(decimal value) => Value = value;

    public static InvoiceItemsPrice Create(decimal value, string fieldName)
    {
        if (value < 0)
            throw new ArgumentException($"El monto de {fieldName} no puede ser negativo.");
            
        return new InvoiceItemsPrice(value);
    }
}