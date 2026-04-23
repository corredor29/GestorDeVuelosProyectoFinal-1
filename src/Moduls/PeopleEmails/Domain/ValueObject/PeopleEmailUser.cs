using System;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;

public sealed record PersonEmailsUser
{
    public string Value { get; }

    private PersonEmailsUser(string value) => Value = value;

    public static PersonEmailsUser Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El usuario del email es obligatorio.");

        var trimmed = value.Trim().ToLowerInvariant();

        if (trimmed.Contains("@"))
            throw new ArgumentException("El usuario del email no debe incluir el simbolo '@'.");

        if (trimmed.Length > 100)
            throw new ArgumentException("El usuario del email no puede superar 100 caracteres.");

        return new PersonEmailsUser(trimmed);
    }
}
