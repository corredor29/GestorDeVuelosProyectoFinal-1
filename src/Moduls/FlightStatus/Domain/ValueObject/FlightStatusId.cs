namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;

public sealed class FlightStatusId
{
    public int Value { get; }

    private FlightStatusId(int value) => Value = value;

    public static FlightStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del estado de vuelo no es valido.", nameof(value));

        return new FlightStatusId(value);
    }

    public override string ToString() => Value.ToString();
}
