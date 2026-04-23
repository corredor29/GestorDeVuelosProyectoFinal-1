namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;

public sealed class CabinTypesName
{
    public string Value { get; }
    private CabinTypesName(string value) => Value = value;

    public static CabinTypesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de cabina no puede estar vacío", nameof(value));

        if (value.Length > 100 || value.Length < 2)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre debe tener entre 2 y 100 caracteres.");
        
        if (value.All(char.IsDigit))
            throw new ArgumentException("El nombre del tipo de cabina no puede contener solo números", nameof(value));

        return new CabinTypesName(value.Trim());
    }
    public override string ToString() => Value.ToString();
}