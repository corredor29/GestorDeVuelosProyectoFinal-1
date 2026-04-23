using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate;

public sealed class FlightStatusTransition
{
    public FlightStatusTransitionsId? Id { get; private set; }
    public FlightStatusId FromStatusId { get; private set; } = null!;
    public FlightStatusId ToStatusId { get; private set; } = null!;

    private FlightStatusTransition() { }

    private FlightStatusTransition(
        FlightStatusTransitionsId? id,
        FlightStatusId fromStatusId,
        FlightStatusId toStatusId)
    {
        Id = id;
        FromStatusId = fromStatusId;
        ToStatusId = toStatusId;
    }

    public static FlightStatusTransition Create(int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        return new FlightStatusTransition(
            id: null,
            fromStatusId: FlightStatusId.Create(fromStatusId),
            toStatusId: FlightStatusId.Create(toStatusId));
    }

    public static FlightStatusTransition FromPrimitives(int id, int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        return new FlightStatusTransition(
            id: FlightStatusTransitionsId.Create(id),
            fromStatusId: FlightStatusId.Create(fromStatusId),
            toStatusId: FlightStatusId.Create(toStatusId));
    }

    public void Update(int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        FromStatusId = FlightStatusId.Create(fromStatusId);
        ToStatusId = FlightStatusId.Create(toStatusId);
    }

    private static void EnsureDifferentStatuses(int fromStatusId, int toStatusId)
    {
        if (fromStatusId == toStatusId)
            throw new ArgumentException("El estado origen no puede ser igual al estado destino.");
    }
}
