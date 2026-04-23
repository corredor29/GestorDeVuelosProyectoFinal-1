namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightsRouteId
{
    public int Value { get; }

    private FlightsRouteId(int value) => Value = value;

    public static FlightsRouteId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("route_id no es valido.");
        return new FlightsRouteId(value);
    }
}
