namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

public sealed class SeasonsDescription
{
    public string? Value { get; }

    private SeasonsDescription(string? value) => Value = value;

    public static SeasonsDescription Create(string? value)
    {
        if (value != null && value.Length > 150)
            throw new ArgumentException("La descripcion no puede superar los 150 caracteres.");

        return new SeasonsDescription(value?.Trim());
    }
}
