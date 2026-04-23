using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.UseCases;

public sealed class GetBookingStatusTransitionsUseCase
{
    private readonly IBookingStatusTransitionsRepository _repository;

    public GetBookingStatusTransitionsUseCase(IBookingStatusTransitionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BookingStatusTransition>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<BookingStatusTransition?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(BookingStatusTransitionsId.Create(id), cancellationToken);

    public Task<IEnumerable<BookingStatusTransition>> GetByFromStatusIdAsync(int fromStatusId, CancellationToken cancellationToken = default)
        => _repository.GetByFromStatusIdAsync(BookingStatusesId.Create(fromStatusId), cancellationToken);

    public Task<bool> ValidateTransitionAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _repository.ValidateTransitionAsync(BookingStatusesId.Create(fromStatusId), BookingStatusesId.Create(toStatusId), cancellationToken);
}
