namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

public sealed class RatesValidTo
{
    public DateOnly? Value { get; }

    private RatesValidTo(DateOnly? value) => Value = value;

    public static RatesValidTo Create(DateOnly? value)
    {
        return new RatesValidTo(value);
    }
}
