namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;

public sealed class StreetTypeName
{
    public string Value { get; }
    private StreetTypeName(string value) => Value = value;

    public static StreetTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre no puede estar vacío");
        return new StreetTypeName(value.Trim());
    }
}