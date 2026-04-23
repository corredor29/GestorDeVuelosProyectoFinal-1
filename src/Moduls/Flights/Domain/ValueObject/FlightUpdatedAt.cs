namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightUpdatedAt
{
    public DateTime Value { get; }

    private FlightUpdatedAt(DateTime value) => Value = value;

    public static FlightUpdatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("updated_at no es valido.");
        return new FlightUpdatedAt(value);
    }

    public static FlightUpdatedAt FromPersistence(DateTime value) => new(value);
}
