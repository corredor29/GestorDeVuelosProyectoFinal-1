namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;

public sealed class StreetTypeId 
{
    public int Value { get; }
    private StreetTypeId(int value) => Value = value;

    public static StreetTypeId Create(int value) => new StreetTypeId(value);
}