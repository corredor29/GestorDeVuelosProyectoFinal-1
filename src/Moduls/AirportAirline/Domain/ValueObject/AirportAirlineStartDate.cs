namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;

public sealed record AirportAirlineStartDate
{
    public DateTime Value { get; }

    private AirportAirlineStartDate(DateTime value) => Value = value;

    public static AirportAirlineStartDate Create(DateTime value)
    {
        if (value == default)
            throw new ArgumentException("La fecha de inicio no es válida.");

        return new AirportAirlineStartDate(value.Date);
    }
}
