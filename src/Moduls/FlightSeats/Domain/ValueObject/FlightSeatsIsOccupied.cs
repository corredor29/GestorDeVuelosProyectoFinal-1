namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;

public sealed class FlightSeatsIsOccupied
{
    public bool Value { get; }
    private FlightSeatsIsOccupied(bool value)
    {
        Value = value;
    }
    public static FlightSeatsIsOccupied Create(bool value)
    {
        return new FlightSeatsIsOccupied(value);
    }
    public override string ToString() => Value.ToString();
}
