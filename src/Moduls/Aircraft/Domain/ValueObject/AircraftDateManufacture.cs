namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;

public sealed record AircraftDateManufacture
{
    public DateTime? Value { get; }

    private AircraftDateManufacture(DateTime? value) => Value = value;

    public static AircraftDateManufacture Create(DateTime? value)
    {
        if (value.HasValue && value.Value.Date > DateTime.UtcNow.Date)
            throw new ArgumentException("La fecha de fabricación no puede estar en el futuro.", nameof(value));

        return new AircraftDateManufacture(value?.Date);
    }

    public override string ToString() => Value?.ToString("yyyy-MM-dd") ?? "-";
}
