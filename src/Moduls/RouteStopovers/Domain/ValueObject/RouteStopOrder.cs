namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

/// <summary>Mapea la columna <c>stop_order</c>, unica junto con <c>route_id</c>.</summary>
public sealed class RouteStopOrder
{
    public int Value { get; }

    private RouteStopOrder(int value) => Value = value;

    public static RouteStopOrder Create(int value)
    {
        if (value < 1)
            throw new ArgumentException("stop_order debe ser al menos 1.", nameof(value));

        return new RouteStopOrder(value);
    }
}
