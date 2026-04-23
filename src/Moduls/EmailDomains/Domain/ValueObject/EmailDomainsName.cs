using System;

namespace GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;

public sealed record EmailDomainName
{
    public string Value { get; }
    private EmailDomainName(string value) => Value = value;

    public static EmailDomainName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre del dominio es obligatorio.");

        var trimmed = value.Trim().ToLower();

        if (trimmed.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "El dominio no puede superar los 100 caracteres.");

        if (!trimmed.Contains('.'))
            throw new ArgumentException("El formato del dominio no es válido (debe incluir un punto, ej: gmail.com).");

        return new EmailDomainName(trimmed);
    }
}