namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsAircraftId
{
    public int Value { get; }

    private FlightsAircraftId(int value) => Value = value;

    public static FlightsAircraftId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("aircraft_id no es valido.");
        return new FlightsAircraftId(value);
    }
}
