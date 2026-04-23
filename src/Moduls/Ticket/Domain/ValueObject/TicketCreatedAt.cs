using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

public sealed class TicketCreatedAt
{
    public DateTime Value { get; }

    private TicketCreatedAt(DateTime value) => Value = value;

    public static TicketCreatedAt Create(DateTime value)
    { // verifica que la fecha no venga vacia
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de creación del ticket no es válida", nameof(value));

        // la creación no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de creación del ticket no puede ser en el pasado", nameof(value));

        return new TicketCreatedAt(value);
    }
}
