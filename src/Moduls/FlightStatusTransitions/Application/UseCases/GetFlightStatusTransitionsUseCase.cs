using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;
using DomainTransition = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate.FlightStatusTransition;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.UseCases;

public sealed class GetFlightStatusTransitionsUseCase
{
    private readonly IFlightStatusTransitionsRepository _repository;

    public GetFlightStatusTransitionsUseCase(IFlightStatusTransitionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<DomainTransition>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<DomainTransition?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(FlightStatusTransitionsId.Create(id), cancellationToken);

    public Task<IEnumerable<DomainTransition>> GetByFromStatusIdAsync(int fromStatusId, CancellationToken cancellationToken = default)
        => _repository.GetByFromStatusIdAsync(FlightStatusId.Create(fromStatusId), cancellationToken);

    public Task<bool> ValidateTransitionAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _repository.ValidateTransitionAsync(FlightStatusId.Create(fromStatusId), FlightStatusId.Create(toStatusId), cancellationToken);
}
