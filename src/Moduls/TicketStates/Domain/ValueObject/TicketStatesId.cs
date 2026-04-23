using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

public sealed class TicketStatesId
{
    public int Value { get; }

    private TicketStatesId(int value) => Value = value;

    public static TicketStatesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("The ticket status ID is invalid", nameof(value));

        return new TicketStatesId(value);
    }
}
