namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;

public interface IBookingStatusTransitionRepository
{
    Task<bool> IsTransitionAllowedAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<int>> GetAllowedToStatusIdsAsync(int fromStatusId, CancellationToken cancellationToken = default);
}
