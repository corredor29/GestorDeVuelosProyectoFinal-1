using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class GetAllAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAllAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftAggregate>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);
}
