namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;

public sealed class AvailabilityStatesId
{
    public int Value { get; }

    private AvailabilityStatesId(int value) => Value = value;

    public static AvailabilityStatesId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del estado de disponibilidad no es válido.", nameof(value));

        return new AvailabilityStatesId(value);
    }
}
