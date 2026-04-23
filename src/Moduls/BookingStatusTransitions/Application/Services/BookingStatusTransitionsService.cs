using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.UseCases;
using DomainTransition = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate.BookingStatusTransition;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.Services;

public sealed class BookingStatusTransitionsService : IBookingStatusTransitionsService
{
    private readonly GetBookingStatusTransitionsUseCase _get;
    private readonly CreateBookingStatusTransitionUseCase _create;
    private readonly UpdateBookingStatusTransitionUseCase _update;
    private readonly DeleteBookingStatusTransitionUseCase _delete;

    public BookingStatusTransitionsService(
        GetBookingStatusTransitionsUseCase get,
        CreateBookingStatusTransitionUseCase create,
        UpdateBookingStatusTransitionUseCase update,
        DeleteBookingStatusTransitionUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<DomainTransition>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<DomainTransition?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<IEnumerable<DomainTransition>> GetByFromStatusIdAsync(int fromStatusId, CancellationToken cancellationToken = default)
        => _get.GetByFromStatusIdAsync(fromStatusId, cancellationToken);

    public Task<bool> ValidateTransitionAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _get.ValidateTransitionAsync(fromStatusId, toStatusId, cancellationToken);

    public Task CreateAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(fromStatusId, toStatusId, cancellationToken);

    public Task UpdateAsync(int id, int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, fromStatusId, toStatusId, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);
}
