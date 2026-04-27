using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Repositories;

public interface IWaitingListRepository
{
    Task<WaitingLis?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WaitingLis>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WaitingLis>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int bookingId, int flightId, CancellationToken cancellationToken = default);

    /// Devuelve el primer candidato en espera para un vuelo,
    /// ordenado por prioridad (mayor = primero) y luego por fecha de solicitud.
    Task<WaitingLis?> GetNextCandidateAsync(int flightId, CancellationToken cancellationToken = default);

    Task SaveAsync(WaitingLis entry, CancellationToken cancellationToken = default);
    Task UpdateAsync(WaitingLis entry, CancellationToken cancellationToken = default);
}