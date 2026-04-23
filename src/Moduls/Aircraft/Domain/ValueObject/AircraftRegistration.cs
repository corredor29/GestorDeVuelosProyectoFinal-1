namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;

public sealed record AircraftRegistration
{
    public string Value { get; }

    private AircraftRegistration(string value) => Value = value;

    public static AircraftRegistration Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La matrícula de la aeronave no puede estar vacía.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length > 20)
            throw new ArgumentOutOfRangeException(nameof(value), "La matrícula no puede superar los 20 caracteres.");

        return new AircraftRegistration(normalized);
    }

    public override string ToString() => Value;
}
