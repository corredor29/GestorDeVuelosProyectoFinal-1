using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;

public sealed class PersonEmailsId
{
    public int Value { get; }

    private PersonEmailsId(int value) => Value = value;

    public static PersonEmailsId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del email de la persona no es valido.", nameof(value));

        return new PersonEmailsId(value);
    }

    public override string ToString() => Value.ToString();
}

