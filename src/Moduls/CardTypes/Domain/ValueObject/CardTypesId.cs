using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;

public sealed class CardTypesId
{
    public int Value { get; }

    private CardTypesId(int value) => Value = value;

    public static CardTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de tarjeta no es válido", nameof(value));

        return new CardTypesId(value);
    }
}
