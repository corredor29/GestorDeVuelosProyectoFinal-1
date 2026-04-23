using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class GetAircraftModelsByManufacturerIdUseCase
{
    private readonly IAircraftModelsRepository _repository;

    public GetAircraftModelsByManufacturerIdUseCase(IAircraftModelsRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftModel>> ExecuteAsync(int manufacturerId, CancellationToken cancellationToken = default)
        => _repository.FindByManufacturerIdAsync(AircraftManufacturersId.Create(manufacturerId), cancellationToken);
}
