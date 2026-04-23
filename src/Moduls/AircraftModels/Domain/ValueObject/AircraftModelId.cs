namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelId
{
    public int Value { get; }
    private AircraftModelId(int value) => Value = value;
    public static AircraftModelId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El ID del modelo no es válido.", nameof(value));
        return new AircraftModelId(value);
    }
    public override string ToString() => Value.ToString();
}