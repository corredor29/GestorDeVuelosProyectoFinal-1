namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

public sealed record RegionType
{
    public string Value { get; }

    private RegionType(string value) => Value = value;

    public static RegionType Create(string value)
    {
        // Validación básica: que no sea nulo, vacío o solo espacios
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El tipo de región es obligatorio.", nameof(value));

        // Validación de longitud: para que coincida con tu VARCHAR(30) de SQL
        if (value.Trim().Length > 30)
            throw new ArgumentOutOfRangeException(nameof(value), "El tipo de región no puede superar los 30 caracteres.");

        // Guardamos el valor normalizado (opcionalmente en mayúscula o Trim)
        return new RegionType(value.Trim());
    }

    public override string ToString() => Value;
}