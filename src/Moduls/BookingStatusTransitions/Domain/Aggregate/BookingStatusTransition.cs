using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate;

public sealed class BookingStatusTransition
{
    public BookingStatusTransitionsId? Id { get; private set; }
    public BookingStatusesId FromStatusId { get; private set; } = null!;
    public BookingStatusesId ToStatusId { get; private set; } = null!;

    private BookingStatusTransition() { }

    private BookingStatusTransition(
        BookingStatusTransitionsId? id,
        BookingStatusesId fromStatusId,
        BookingStatusesId toStatusId)
    {
        Id = id;
        FromStatusId = fromStatusId;
        ToStatusId = toStatusId;
    }

    public static BookingStatusTransition Create(int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        return new BookingStatusTransition(
            id: null,
            fromStatusId: BookingStatusesId.Create(fromStatusId),
            toStatusId: BookingStatusesId.Create(toStatusId));
    }

    public static BookingStatusTransition FromPrimitives(int id, int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        return new BookingStatusTransition(
            id: BookingStatusTransitionsId.Create(id),
            fromStatusId: BookingStatusesId.Create(fromStatusId),
            toStatusId: BookingStatusesId.Create(toStatusId));
    }

    public void Update(int fromStatusId, int toStatusId)
    {
        EnsureDifferentStatuses(fromStatusId, toStatusId);

        FromStatusId = BookingStatusesId.Create(fromStatusId);
        ToStatusId = BookingStatusesId.Create(toStatusId);
    }

    private static void EnsureDifferentStatuses(int fromStatusId, int toStatusId)
    {
        if (fromStatusId == toStatusId)
            throw new ArgumentException("El estado origen no puede ser igual al estado destino.");
    }
}
