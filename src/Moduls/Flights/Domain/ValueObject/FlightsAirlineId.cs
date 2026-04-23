namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsAirlineId
{
    public int Value { get; }

    private FlightsAirlineId(int value) => Value = value;

    public static FlightsAirlineId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("airline_id no es valido.");
        return new FlightsAirlineId(value);
    }
}
