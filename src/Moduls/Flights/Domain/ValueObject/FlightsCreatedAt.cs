namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsCreatedAt
{
    public DateTime Value { get; }

    private FlightsCreatedAt(DateTime value) => Value = value;

    public static FlightsCreatedAt Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("created_at no es valido.");
        return new FlightsCreatedAt(value);
    }

    public static FlightsCreatedAt FromPersistence(DateTime value) => new(value);
}
