namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

public sealed class AirportsId
{
    public int Value { get; }

    private AirportsId(int value) => Value = value;

    public static AirportsId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del aeropuerto no es válido.", nameof(value));

        return new AirportsId(value);
    }
}
