using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

public sealed class TicketId
{
    public int Value { get; }

    private TicketId(int value) => Value = value;

    public static TicketId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del ticket no es válido", nameof(value));

        return new TicketId(value);
    }
}
