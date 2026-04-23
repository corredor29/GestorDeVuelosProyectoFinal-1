namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

public sealed class UsersIsActive
{
    public bool Value { get; }
    private UsersIsActive(bool value) => Value = value;

    public static UsersIsActive Create(bool value) => new UsersIsActive(value);
}