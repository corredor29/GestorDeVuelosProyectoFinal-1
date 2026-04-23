namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;

public sealed class AirlinesId
{
    public int Value { get; }

    private AirlinesId(int value) => Value = value;

    public static AirlinesId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id de la aerolínea no es válido.", nameof(value));

        return new AirlinesId(value);
    }
}
