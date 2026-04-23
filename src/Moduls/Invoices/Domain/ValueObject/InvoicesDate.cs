using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

public sealed class InvoicesDate
{
    public DateTime Value { get; }
    private InvoicesDate(DateTime value) => Value = value;

    public static InvoicesDate Create(DateTime value)
    {
        // Validamos que no sea una fecha por defecto del sistema
        if (value == default)
            throw new ArgumentException("La fecha de emisión no es válida.");
            
        return new InvoicesDate(value);
    }
}