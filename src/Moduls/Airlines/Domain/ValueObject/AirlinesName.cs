namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;

public sealed class AirlinesName
{
    public string Value { get; }

    private AirlinesName(string value) => Value = value;

    public static AirlinesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la aerolínea no puede estar vacío.", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2 || normalized.Length > 150)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre de la aerolínea debe tener entre 2 y 150 caracteres.");

        if (normalized.All(char.IsDigit))
            throw new ArgumentException("El nombre de la aerolínea no puede contener solo números.", nameof(value));

        return new AirlinesName(normalized);
    }
}
