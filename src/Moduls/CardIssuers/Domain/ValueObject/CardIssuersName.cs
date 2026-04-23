using System;


namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;

public sealed class CardIssuersName
{
    public string Value { get; }
    private CardIssuersName(string value) => Value = value;

    public static CardIssuersName Create(string value)
    {
        // Validación de nulos o vacíos
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentException("El nombre del emisor de tarjetas no puede estar vacío.");

        // Validación de longitud
        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del emisor de tarjetas debe tener entre 2 y 100 caracteres.");
        
        // Validación de que no sean solo números
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del emisor de tarjetas no puede contener solo números.", nameof(value));

        // Único return al final, una vez pasadas todas las pruebas
        return new CardIssuersName(value.Trim());
    }
}