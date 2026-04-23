using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class GetAircraftModelByIdUseCase
{
    private readonly IAircraftModelsRepository _repository;

    public GetAircraftModelByIdUseCase(IAircraftModelsRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftModel?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        => _repository.FindByIdAsync(AircraftModelId.Create(id), cancellationToken);
}
