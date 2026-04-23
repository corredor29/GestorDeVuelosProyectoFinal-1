using System;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;

public sealed class CabinConfigurationId
{
    public int Value { get; }
    private CabinConfigurationId(int value) => Value = value;
    public static CabinConfigurationId Create(int value)
    {
        if (value < 0) throw new ArgumentException("El ID debe ser mayor o igual a cero.");
        return new CabinConfigurationId(value);
    }
    public override string ToString() => Value.ToString();
}