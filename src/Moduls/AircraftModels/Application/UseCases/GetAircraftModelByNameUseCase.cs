using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class GetAircraftModelByNameUseCase
{
    private readonly IAircraftModelsRepository _repository;

    public GetAircraftModelByNameUseCase(IAircraftModelsRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftModel?> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        => _repository.FindByNameAsync(AircraftModelName.Create(name), cancellationToken);
}
