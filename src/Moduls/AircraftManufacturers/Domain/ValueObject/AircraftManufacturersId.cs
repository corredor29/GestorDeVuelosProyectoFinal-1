namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;

public sealed record AircraftManufacturersId
{
    public int Value { get; }
    private AircraftManufacturersId(int value) => Value = value;

    public static AircraftManufacturersId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del fabricante de aviones debe ser un número positivo.", nameof(value));

        return new AircraftManufacturersId(value);
    }

    public override string ToString() => Value.ToString();
}
