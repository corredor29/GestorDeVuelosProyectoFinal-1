namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

public sealed class InvoicesNumber
{
    public string Value { get; }
    private InvoicesNumber(string value) => Value = value;

    public static InvoicesNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de factura no puede estar vacío.");
        
        if (value.Length > 30)
            throw new ArgumentException("El número de factura no puede exceder los 30 caracteres.");

        return new InvoicesNumber(value.Trim());
    }
}