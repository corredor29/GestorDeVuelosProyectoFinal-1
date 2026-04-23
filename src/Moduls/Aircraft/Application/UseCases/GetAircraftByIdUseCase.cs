using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class GetAircraftByIdUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByIdUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftAggregate?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(AircraftId.Create(id), cancellationToken);
}
