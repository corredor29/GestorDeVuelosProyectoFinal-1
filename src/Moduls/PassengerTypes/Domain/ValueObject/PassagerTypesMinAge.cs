namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

public sealed class PassengerMinAge
{
    public int? Value { get; }

    private PassengerMinAge(int? value) => Value = value;

    public static PassengerMinAge Create(int? value)
    {
        if (value.HasValue && value.Value < 0)
            throw new ArgumentException("min_age no puede ser negativa.");

        return new PassengerMinAge(value);
    }
}
