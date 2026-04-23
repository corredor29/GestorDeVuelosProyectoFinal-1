namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;

public sealed class AirportAirlineEndDate
{
    public DateTime? Value { get; }

    private AirportAirlineEndDate(DateTime? value) => Value = value;

    public static AirportAirlineEndDate Create(DateTime? value)
    {
        if (value == default(DateTime))
            return new AirportAirlineEndDate(null);

        return new AirportAirlineEndDate(value?.Date);
    }
}
