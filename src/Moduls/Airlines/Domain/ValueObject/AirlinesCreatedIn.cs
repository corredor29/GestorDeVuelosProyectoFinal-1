namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;

public sealed record AirlinesCreatedIn
{
    public DateTime Value { get; }

    private AirlinesCreatedIn(DateTime value) => Value = value;

    public static AirlinesCreatedIn Create(DateTime value)
    {
        if (value == default)
            throw new ArgumentException("La fecha de creación no es válida.");

        return new AirlinesCreatedIn(value);
    }
}
