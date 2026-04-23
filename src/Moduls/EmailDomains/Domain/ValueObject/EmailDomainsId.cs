using System;

namespace GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;

public sealed class EmailDomainsId
{
    public int Value { get; }

    private EmailDomainsId(int value) => Value = value;

    public static EmailDomainsId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id del dominio de correo no es valido.", nameof(value));

        return new EmailDomainsId(value);
    }

    public override string ToString() => Value.ToString();
}
