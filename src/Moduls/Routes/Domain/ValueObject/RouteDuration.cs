namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

public sealed class RouteDuration
{
    public int? Value { get; }
    private RouteDuration(int? value) => Value = value;
    public static RouteDuration Create(int? value)
    {
        if (value.HasValue && value <= 0)
            throw new ArgumentException("La duración estimada debe ser mayor a cero minutos.");
        return new RouteDuration(value);
    }
}