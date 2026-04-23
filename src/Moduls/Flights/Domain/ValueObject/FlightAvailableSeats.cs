namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightAvailableSeats
{
    public int Value { get; }

    private FlightAvailableSeats(int value) => Value = value;

    public static FlightAvailableSeats Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("available_seats no puede ser negativo.");
        return new FlightAvailableSeats(value);
    }
}
