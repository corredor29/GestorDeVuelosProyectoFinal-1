namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

public sealed class AirportsName
{
    public string Value { get; }

    private AirportsName(string value) => Value = value;

    public static AirportsName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del aeropuerto no puede estar vacío.", nameof(value));

        var normalized = value.Trim();

        if (normalized.Length < 2 || normalized.Length > 150)
            throw new ArgumentOutOfRangeException(nameof(value), "El nombre del aeropuerto debe tener entre 2 y 150 caracteres.");

        if (normalized.All(char.IsDigit))
            throw new ArgumentException("El nombre del aeropuerto no puede contener solo números.", nameof(value));

        return new AirportsName(normalized);
    }
}
