namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

public sealed class RouteStopOversId
{
    public int Value { get; }

    private RouteStopOversId(int value) => Value = value;

    public static RouteStopOversId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la escala no es válido.", nameof(value));

        return new RouteStopOversId(value);
    }

    public override string ToString() => Value.ToString();
}
