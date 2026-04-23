using System;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;

public sealed class CardIssuersIssuerNumber
{
    public string Value { get; }
    private CardIssuersIssuerNumber(string value) => Value = value;

    public static CardIssuersIssuerNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número del emisor de tarjeta no puede estar vacío.");

        // Normalmente los Issuer Numbers (IIN/BIN) tienen entre 6 y 8 dígitos
        if (value.Length < 4 || value.Length > 15)
            throw new ArgumentException("El número del emisor de tarjeta debe tener una longitud válida.");

        return new CardIssuersIssuerNumber(value.Trim());
    }
}