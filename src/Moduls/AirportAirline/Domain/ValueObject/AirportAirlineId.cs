namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;

public sealed class AirportAirlineId
{
    public int Value { get; }

    private AirportAirlineId(int value) => Value = value;

    public static AirportAirlineId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id de la operación aeropuerto-aerolínea no es válido.", nameof(value));

        return new AirportAirlineId(value);
    }
}
