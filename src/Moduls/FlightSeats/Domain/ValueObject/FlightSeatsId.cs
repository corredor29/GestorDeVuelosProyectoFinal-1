namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;

public sealed class FlightSeatsId
{
    public int Value { get; }
    private FlightSeatsId(int value) => Value = value;
    public static FlightSeatsId Create(int value)
    {
        if (value <= 0) 
            throw new ArgumentException("El id de asientos es válido.", nameof(value));

        return new FlightSeatsId(value);
    }
    public override string ToString() => Value.ToString();
}
