using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Services;

public sealed class AircraftModelService : IAircraftModelsService
{
    private readonly GetAllAircraftModelsUseCase _getAll;
    private readonly GetAircraftModelByIdUseCase _getById;
    private readonly GetAircraftModelByNameUseCase _getByName;
    private readonly GetAircraftModelsByManufacturerIdUseCase _getByManufacturerId;
    private readonly CreateAircraftModelsUseCase _create;
    private readonly UpdateAircraftModelsUseCase _update;
    private readonly DeleteAircraftModelsUseCase _delete;

    public AircraftModelService(
        GetAllAircraftModelsUseCase getAll,
        GetAircraftModelByIdUseCase getById,
        GetAircraftModelByNameUseCase getByName,
        GetAircraftModelsByManufacturerIdUseCase getByManufacturerId,
        CreateAircraftModelsUseCase create,
        UpdateAircraftModelsUseCase update,
        DeleteAircraftModelsUseCase delete)
    {
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _getByManufacturerId = getByManufacturerId;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IReadOnlyCollection<AircraftModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => _getAll.ExecuteAsync(cancellationToken);

    public Task<AircraftModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _getById.ExecuteAsync(id, cancellationToken);

    public Task<AircraftModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _getByName.ExecuteAsync(name, cancellationToken);

    public Task<IReadOnlyCollection<AircraftModel>> GetByManufacturerIdAsync(int manufacturerId, CancellationToken cancellationToken = default)
        => _getByManufacturerId.ExecuteAsync(manufacturerId, cancellationToken);

    public Task<AircraftModel> CreateAsync(
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(manufacturerId, name, maxCapacity, weight, fuelConsumption, cruiseSpeed, cruiseAltitude, cancellationToken);

    public Task<AircraftModel> UpdateAsync(
        int id,
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, manufacturerId, name, maxCapacity, weight, fuelConsumption, cruiseSpeed, cruiseAltitude, cancellationToken);

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteAsync(id, cancellationToken);
}
