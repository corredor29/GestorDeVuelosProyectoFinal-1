using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Interfaces;

public interface IAircraftModelsService
{
    Task<AircraftModel> CreateAsync(
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default);

    Task<AircraftModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AircraftModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftModel>> GetByManufacturerIdAsync(int manufacturerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AircraftModel> UpdateAsync(
        int id,
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
