namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

public sealed class UsersUsername
{
    public string Value { get; }
    private UsersUsername(string value) => Value = value;

    public static UsersUsername Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        
        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("El nombre de usuario debe tener entre 3 y 50 caracteres.");

        return new UsersUsername(value.Trim());
    }
}