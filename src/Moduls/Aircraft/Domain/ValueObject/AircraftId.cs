namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;

public sealed record AircraftId
{
    public int Value { get; }

    private AircraftId(int value) => Value = value;

    public static AircraftId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del avión no es válido.", nameof(value));

        return new AircraftId(value);
    }

    public override string ToString() => Value.ToString();
}
