using System;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;

public sealed class CheckinState
{
    public CheckinStatesId Id { get; private set; } = null!;
    public CheckinStatesName Name { get; private set; } = null!;

    private CheckinState() { }

    public static CheckinState Create(int id, string name)
    {
        return new CheckinState
        {
            Id = CheckinStatesId.Create(id),
            Name = CheckinStatesName.Create(name)
        };
    }
    internal void SetId(int id)
    {
        Id = CheckinStatesId.Create(id);
    }

    public void UpdateName(string newName)
    {
        Name = CheckinStatesName.Create(newName);
    }
}