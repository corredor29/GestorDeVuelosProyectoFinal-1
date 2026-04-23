using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class GetAircraftByRegistrationUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByRegistrationUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftAggregate?> ExecuteAsync(string registration, CancellationToken cancellationToken = default)
        => _repository.GetByRegistrationAsync(AircraftRegistration.Create(registration), cancellationToken);
}
