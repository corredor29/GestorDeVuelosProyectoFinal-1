namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

public sealed class PassengerMaxAge
{
    public int? Value { get; }

    private PassengerMaxAge(int? value) => Value = value;

    public static PassengerMaxAge Create(int? value)
    {
        if (value.HasValue && value.Value < 0)
            throw new ArgumentException("max_age no puede ser negativa.");

        return new PassengerMaxAge(value);
    }
}
