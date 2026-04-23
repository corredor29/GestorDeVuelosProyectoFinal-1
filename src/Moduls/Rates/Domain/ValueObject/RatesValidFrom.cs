namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

public sealed class RatesValidFrom
{
    public DateOnly? Value { get; }

    private RatesValidFrom(DateOnly? value) => Value = value;

    public static RatesValidFrom Create(DateOnly? value)
    {
        return new RatesValidFrom(value);
    }
}
