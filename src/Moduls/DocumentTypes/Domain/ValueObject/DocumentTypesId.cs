using System;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

public sealed class DocumentTypesId 
{
    public int Value { get; }

    private DocumentTypesId(int value) => Value = value;

    public static DocumentTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de documento no es válido", nameof(value));

        return new DocumentTypesId(value);
    }
}
