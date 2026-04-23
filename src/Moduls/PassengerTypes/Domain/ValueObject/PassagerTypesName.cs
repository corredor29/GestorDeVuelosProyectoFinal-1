namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

public sealed class PassengerTypesName
{
    public const int MaxLength = 50;

    public string Value { get; }

    private PassengerTypesName(string value) => Value = value;

    public static PassengerTypesName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del tipo de pasajero es obligatorio.");

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"El nombre no puede superar {MaxLength} caracteres.");

        return new PassengerTypesName(trimmed);
    }

    public override string ToString() => Value;
}
