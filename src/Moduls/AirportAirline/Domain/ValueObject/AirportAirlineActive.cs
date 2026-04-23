namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;

public sealed record AirportAirlineIsActive
{
    public bool Value { get; }

    private AirportAirlineIsActive(bool value) => Value = value;

    public static AirportAirlineIsActive Create(bool value) => new(value);
}
