namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

public sealed class FlightRescheduledAt
{
    public DateTime? Value { get; }

    private FlightRescheduledAt(DateTime? value) => Value = value;

    public static FlightRescheduledAt CreateNullable(DateTime? value) => new(value);

    public static FlightRescheduledAt FromPersistence(DateTime? value) => new(value);
}
