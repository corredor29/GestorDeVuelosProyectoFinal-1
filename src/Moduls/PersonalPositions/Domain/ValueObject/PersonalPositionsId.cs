namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

public sealed class PersonalPositionsId
{
    public int Value { get; }

    private PersonalPositionsId(int value) => Value = value;

    public static PersonalPositionsId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del cargo no es válido.", nameof(value));

        return new PersonalPositionsId(value);
    }
}
