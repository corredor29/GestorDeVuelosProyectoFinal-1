namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsFlightStatusId
{
    public int Value { get; }

    private FlightsFlightStatusId(int value) => Value = value;

    public static FlightsFlightStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("flight_status_id no es valido.");
        return new FlightsFlightStatusId(value);
    }
}
