using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

public sealed class TicketCode
{
    public string Value { get; }

    private TicketCode(string value) => Value = value;

    public static TicketCode Create(string value)
    {
        if (value.Length > 30)
            throw new ArgumentException("El código del ticket no puede superar los 30 caracteres", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código del ticket no es válido", nameof(value));

        return new TicketCode(value);
    }
}
