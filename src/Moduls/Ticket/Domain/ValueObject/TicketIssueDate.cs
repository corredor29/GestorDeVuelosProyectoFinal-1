using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

public sealed class TicketIssueDate
{
    public DateTime Value { get; }

    private TicketIssueDate(DateTime value) => Value = value;

    public static TicketIssueDate Create(DateTime value)
    { // verifica que la fecha no venga vacia
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de emisión del ticket no es válida", nameof(value));

        // la emisión no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de emisión del ticket no puede ser en el pasado", nameof(value));

        return new TicketIssueDate(value);
    }
}
