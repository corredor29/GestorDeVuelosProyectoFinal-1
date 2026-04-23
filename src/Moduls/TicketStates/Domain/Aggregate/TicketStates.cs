using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;

public sealed class TicketState
{
    public TicketStatesId Id { get; private set; } = null!;
    public TicketStatesName Name { get; private set; } = null!;

    private TicketState() { }

    private TicketState(
        TicketStatesId id,
        TicketStatesName name)
    {
        Id = id;
        Name = name;
    }

    public static TicketState Create(
        int id,
        string name)
    {
        return new TicketState(
            TicketStatesId.Create(id),
            TicketStatesName.Create(name)
        );
    }
    internal void SetId(int id)
    {
        Id = TicketStatesId.Create(id);
    }

    public void UpdateName(string newName)
    {
        Name = TicketStatesName.Create(newName);
    }  
}
