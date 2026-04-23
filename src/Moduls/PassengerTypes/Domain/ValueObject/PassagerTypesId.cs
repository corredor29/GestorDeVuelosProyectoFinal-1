namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

public sealed class PassengerTypesId
{
    public int Value { get; }

    private PassengerTypesId(int value) => Value = value;

    public static PassengerTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tipo de pasajero no es valido.");

        return new PassengerTypesId(value);
    }

    public override string ToString() => Value.ToString();
}
