using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

public sealed class TicketUpdateAt
{
    public DateTime Value { get; }

    private TicketUpdateAt(DateTime value) => Value = value;

    public static TicketUpdateAt Create(DateTime value)
    { // verifica que la fecha no venga vacia
        if (value == DateTime.MinValue)
            throw new ArgumentException("La fecha de actualización del ticket no es válida", nameof(value));

        // la actualización no puede ser en el pasado
        if (value < DateTime.UtcNow)
            throw new ArgumentException("La fecha de actualización del ticket no puede ser en el pasado", nameof(value));

        return new TicketUpdateAt(value);
    }
}
