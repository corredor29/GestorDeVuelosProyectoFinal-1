namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;

public sealed record AirlinesUpdatedIn
{
    public DateTime Value { get; }

    private AirlinesUpdatedIn(DateTime value) => Value = value;

    public static AirlinesUpdatedIn Create(DateTime value)
    {
        if (value == default)
            throw new ArgumentException("La fecha de actualización no es válida.");

        return new AirlinesUpdatedIn(value);
    }
}
