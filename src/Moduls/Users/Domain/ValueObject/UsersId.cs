using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

public sealed class UsersId
{
    public int Value { get; }
    private UsersId(int value) => Value = value;

    public static UsersId Create(int value)
    {
            if (value <= 0)
            throw new ArgumentException("El id del usuario no es válido.");
        return new UsersId(value);
    }
}