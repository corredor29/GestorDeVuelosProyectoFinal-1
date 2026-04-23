namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightDepartureTime
{
    public DateTime Value { get; }

    private FlightDepartureTime(DateTime value) => Value = value;

    public static FlightDepartureTime Create(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La hora de salida del vuelo no es valida.");
        return new FlightDepartureTime(value);
    }

    /// <summary>Reconstruccion desde persistencia (sin reglas de negocio de calendario).</summary>
    public static FlightDepartureTime FromPersistence(DateTime value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("La hora de salida del vuelo no es valida.");
        return new FlightDepartureTime(value);
    }
}
