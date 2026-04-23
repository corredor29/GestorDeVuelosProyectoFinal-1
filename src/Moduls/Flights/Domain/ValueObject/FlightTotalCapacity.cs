namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightTotalCapacity
{
    public int Value { get; }

    private FlightTotalCapacity(int value) => Value = value;

    public static FlightTotalCapacity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("total_capacity debe ser mayor que cero.");
        return new FlightTotalCapacity(value);
    }
}
