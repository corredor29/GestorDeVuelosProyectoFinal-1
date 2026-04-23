using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class GetAllAircraftModelsUseCase
{
    private readonly IAircraftModelsRepository _repository;

    public GetAllAircraftModelsUseCase(IAircraftModelsRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.FindAllAsync(cancellationToken);
}
