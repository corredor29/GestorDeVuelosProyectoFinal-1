using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;

public sealed class CardTypesName
{
    public string Value { get; }

    private CardTypesName(string value) => Value = value;

    public static CardTypesName Create(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("El nombre del tipo de tarjeta no puede superar los 50 caracteres", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de tarjeta no es válido", nameof(value));

        return new CardTypesName(value);
    }
}
