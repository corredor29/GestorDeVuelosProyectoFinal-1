using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.UseCases;

public sealed class CreateFlightStatusTransitionUseCase
{
    private readonly IFlightStatusTransitionsRepository _repository;
    private readonly IFlightStatusRepository _flightStatusRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlightStatusTransitionUseCase(
        IFlightStatusTransitionsRepository repository,
        IFlightStatusRepository flightStatusRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _flightStatusRepository = flightStatusRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
    {
        await EnsureStatusesExistAsync(fromStatusId, toStatusId, cancellationToken);

        var duplicate = await _repository.GetByStatusesAsync(FlightStatusId.Create(fromStatusId), FlightStatusId.Create(toStatusId), cancellationToken);
        if (duplicate is not null)
            throw new InvalidOperationException("Ya existe una transicion con el mismo estado origen y destino.");

        var transition = FlightStatusTransition.Create(fromStatusId, toStatusId);
        await _repository.SaveAsync(transition, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureStatusesExistAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken)
    {
        if (await _flightStatusRepository.GetByIdAsync(FlightStatusId.Create(fromStatusId), cancellationToken) is null)
            throw new InvalidOperationException($"No se encontro el estado origen con ID {fromStatusId}.");

        if (await _flightStatusRepository.GetByIdAsync(FlightStatusId.Create(toStatusId), cancellationToken) is null)
            throw new InvalidOperationException($"No se encontro el estado destino con ID {toStatusId}.");
    }
}
