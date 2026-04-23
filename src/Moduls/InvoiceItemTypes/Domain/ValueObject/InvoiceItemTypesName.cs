using System;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

public sealed class InvoiceItemTypesName
{
    public string Value { get; }
    private InvoiceItemTypesName(string value) => Value = value;

    public static InvoiceItemTypesName Create(string value)
    {
        // Validación de nulos o vacíos
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentException("El nombre del tipo de artículo no puede estar vacío.");

        // Validación de longitud
        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del tipo de artículo debe tener entre 2 y 100 caracteres.");
        
        // Validación de que no sean solo números
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del tipo de artículo no puede contener solo números.", nameof(value));

        // Único return al final, una vez pasadas todas las pruebas
        return new InvoiceItemTypesName(value.Trim());
    }
}