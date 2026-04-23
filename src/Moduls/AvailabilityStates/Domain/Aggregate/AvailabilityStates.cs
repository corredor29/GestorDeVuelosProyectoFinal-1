using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;

public sealed class AvailabilityState
{
    public AvailabilityStatesId? Id { get; private set; }
    public AvailabilityStatesName Name { get; private set; } = null!;

    private AvailabilityState() { }

    private AvailabilityState(AvailabilityStatesId? id, AvailabilityStatesName name)
    {
        Id = id;
        Name = name;
    }

    public static AvailabilityState Create(string name)
    {
        return new AvailabilityState(
            id: null,
            name: AvailabilityStatesName.Create(name));
    }

    public static AvailabilityState FromPrimitives(int id, string name)
    {
        return new AvailabilityState(
            id: AvailabilityStatesId.Create(id),
            name: AvailabilityStatesName.Create(name));
    }

    public void Update(string name)
    {
        Name = AvailabilityStatesName.Create(name);
    }
}
