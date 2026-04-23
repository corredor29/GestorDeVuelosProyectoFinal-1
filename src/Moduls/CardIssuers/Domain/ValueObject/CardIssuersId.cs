using System;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;

public sealed class CardIssuersId
{
    public int Value { get; }
    private CardIssuersId(int value) => Value = value;
    public static CardIssuersId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del emisor de tarjetas no es válido.");
        return new CardIssuersId(value);
    }
}