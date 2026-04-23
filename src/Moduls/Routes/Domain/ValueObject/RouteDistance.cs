namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

public sealed class RouteDistance
{
    public int? Value { get; }
    private RouteDistance(int? value) => Value = value;
    public static RouteDistance Create(int? value)
    {
        if (value.HasValue && value <= 0)
            throw new ArgumentException("La distancia debe ser un número positivo.");
        return new RouteDistance(value);
    }
}