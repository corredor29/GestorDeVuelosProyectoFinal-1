using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.UseCases;

public sealed class CreateBookingStatusTransitionUseCase
{
    private readonly IBookingStatusTransitionsRepository _repository;
    private readonly IBookingStatuseRepository _bookingStatusesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingStatusTransitionUseCase(
        IBookingStatusTransitionsRepository repository,
        IBookingStatuseRepository bookingStatusesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _bookingStatusesRepository = bookingStatusesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
    {
        await EnsureStatusesExistAsync(fromStatusId, toStatusId, cancellationToken);

        var duplicate = await _repository.GetByStatusesAsync(
            BookingStatusesId.Create(fromStatusId),
            BookingStatusesId.Create(toStatusId),
            cancellationToken);

        if (duplicate is not null)
            throw new InvalidOperationException("Ya existe una transicion con el mismo estado origen y destino.");

        var transition = BookingStatusTransition.Create(fromStatusId, toStatusId);
        await _repository.SaveAsync(transition, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureStatusesExistAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken)
    {
        if (await _bookingStatusesRepository.GetByIdAsync(BookingStatusesId.Create(fromStatusId)) is null)
            throw new InvalidOperationException($"No se encontro el estado origen con ID {fromStatusId}.");

        if (await _bookingStatusesRepository.GetByIdAsync(BookingStatusesId.Create(toStatusId)) is null)
            throw new InvalidOperationException($"No se encontro el estado destino con ID {toStatusId}.");
    }
}
