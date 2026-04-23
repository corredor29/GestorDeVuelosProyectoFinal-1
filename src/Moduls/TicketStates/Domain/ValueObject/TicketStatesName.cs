using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

public sealed class TicketStatesName
{
    public string Value { get; }

    private TicketStatesName(string value) => Value = value;

    public static TicketStatesName Create(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("The ticket status name cannot exceed 50 characters", nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The ticket status name is invalid", nameof(value));

        return new TicketStatesName(value);
    }
}
