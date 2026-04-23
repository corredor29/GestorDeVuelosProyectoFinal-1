namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

public sealed class InvoiceItemsDescription
{
    public string Value { get; }
    private InvoiceItemsDescription(string value) => Value = value;

    public static InvoiceItemsDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La descripción del ítem es obligatoria.");
        
        if (value.Length > 200)
            throw new ArgumentException("La descripción no puede exceder los 200 caracteres.");

        return new InvoiceItemsDescription(value.Trim());
    }
}