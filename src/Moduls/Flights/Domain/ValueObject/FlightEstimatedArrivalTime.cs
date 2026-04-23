namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightEstimatedArrivalTime
{
    public DateTime Value { get; }

    private FlightEstimatedArrivalTime(DateTime value) => Value = value;

    public static FlightEstimatedArrivalTime Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La hora de llegada estimada no es valida.");
        return new FlightEstimatedArrivalTime(value);
    }

    public static FlightEstimatedArrivalTime FromPersistence(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La hora de llegada estimada no es valida.");
        return new FlightEstimatedArrivalTime(value);
    }
}
