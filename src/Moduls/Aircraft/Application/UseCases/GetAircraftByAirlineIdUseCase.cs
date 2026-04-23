using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class GetAircraftByAirlineIdUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByAirlineIdUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftAggregate>> ExecuteAsync(int airlineId, CancellationToken cancellationToken = default)
        => _repository.GetByAirlineIdAsync(AirlinesId.Create(airlineId), cancellationToken);
}
