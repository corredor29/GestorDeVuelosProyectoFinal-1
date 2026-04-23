namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;

using System;

public sealed class CabinTypesId 
{
    public int Value { get; }

    private CabinTypesId(int value) => Value = value;

    public static CabinTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de cabina debe ser un número positivo", nameof(value));

        return new CabinTypesId(value);
    }
    public override string ToString() => Value.ToString();
}