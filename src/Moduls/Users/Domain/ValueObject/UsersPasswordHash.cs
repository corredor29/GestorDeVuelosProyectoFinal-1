namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

public sealed class UsersPasswordHash
{
    public string Value { get; }
    private UsersPasswordHash(string value) => Value = value;

    public static UsersPasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El hash de la contraseña no puede estar vacío.");

        return new UsersPasswordHash(value);
    }
}