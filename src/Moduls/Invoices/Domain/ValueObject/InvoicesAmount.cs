namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

public sealed class InvoicesAmount
{
    public decimal Value { get; }
    private InvoicesAmount(decimal value) => Value = value;

    public static InvoicesAmount Create(decimal value, string fieldName)
    {
        if (value < 0)
            throw new ArgumentException($"El monto de {fieldName} no puede ser negativo.");
            
        return new InvoicesAmount(value);
    }
}