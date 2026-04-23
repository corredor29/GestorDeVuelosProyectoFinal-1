using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.UseCases;
using DomainTransition = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate.FlightStatusTransition;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Services;

public sealed class FlightStatusTransitionsService : IFlightStatusTransitionsService
{
    private readonly GetFlightStatusTransitionsUseCase _get;
    private readonly CreateFlightStatusTransitionUseCase _create;
    private readonly UpdateFlightStatusTransitionUseCase _update;
    private readonly DeleteFlightStatusTransitionUseCase _delete;

    public FlightStatusTransitionsService(
        GetFlightStatusTransitionsUseCase get,
        CreateFlightStatusTransitionUseCase create,
        UpdateFlightStatusTransitionUseCase update,
        DeleteFlightStatusTransitionUseCase delete)
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
