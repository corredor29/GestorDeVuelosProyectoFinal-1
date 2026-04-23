namespace GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelMaxCapacity
{
    public int Value { get; }
    private AircraftModelMaxCapacity(int value) => Value = value;
    public static AircraftModelMaxCapacity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La capacidad máxima debe ser mayor a cero.", nameof(value));
        if (value > 900)
            throw new ArgumentException("La capacidad máxima no puede superar 900 asientos.", nameof(value));
        return new AircraftModelMaxCapacity(value);
    }
    public override string ToString() => Value.ToString();
}