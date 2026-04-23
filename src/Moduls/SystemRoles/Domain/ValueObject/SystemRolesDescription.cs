namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

public sealed class RolesDescription
{
    public string? Value { get; }
    private RolesDescription(string? value) => Value = value;

    public static RolesDescription Create(string? value)
    {
        if (value != null && value.Length > 150)
            throw new ArgumentException("La descripción del rol no puede exceder los 150 caracteres.");

        return new RolesDescription(value?.Trim());
    }
}