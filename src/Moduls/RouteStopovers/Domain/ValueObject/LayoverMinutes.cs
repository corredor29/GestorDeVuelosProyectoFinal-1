namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

/// <summary>Mapea la columna <c>layover_min</c> (minutos de escala).</summary>
public sealed class LayoverMinutes
{
    public int Value { get; }

    private LayoverMinutes(int value) => Value = value;

    public static LayoverMinutes Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("layover_min no puede ser negativo.", nameof(value));

        return new LayoverMinutes(value);
    }
}
