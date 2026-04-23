using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

public sealed record PhoneCodesId
{
    public int Value { get; }
    private PhoneCodesId(int value) => Value = value;

    public static PhoneCodesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID del código telefónico no puede estar vacío.");

        return new PhoneCodesId(value);
    }
}
