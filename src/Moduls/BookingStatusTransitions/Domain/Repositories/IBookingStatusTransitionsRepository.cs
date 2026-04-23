using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;

public interface IBookingStatusTransitionsRepository
{
    Task<BookingStatusTransition?> GetByIdAsync(BookingStatusTransitionsId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingStatusTransition>> GetByFromStatusIdAsync(BookingStatusesId fromStatusId, CancellationToken cancellationToken = default);
    Task<BookingStatusTransition?> GetByStatusesAsync(BookingStatusesId fromStatusId, BookingStatusesId toStatusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingStatusTransition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(BookingStatusTransition transition, CancellationToken cancellationToken = default);
    Task UpdateAsync(BookingStatusTransition transition, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(BookingStatusTransitionsId id, CancellationToken cancellationToken = default);
    Task<bool> ValidateTransitionAsync(BookingStatusesId fromStatusId, BookingStatusesId toStatusId, CancellationToken cancellationToken = default);
}
