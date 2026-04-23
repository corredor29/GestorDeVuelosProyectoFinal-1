using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;

public interface IAircraftModelsRepository
{
    Task AddAsync(AircraftModel model, CancellationToken cancellationToken = default);
    Task<AircraftModel?> FindByIdAsync(AircraftModelId id, CancellationToken cancellationToken = default);
    Task<AircraftModel?> FindByNameAsync(AircraftModelName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftModel>> FindByManufacturerIdAsync(AircraftManufacturersId manufacturerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftModel>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByManufacturerAndNameAsync(AircraftManufacturersId manufacturerId, AircraftModelName name, CancellationToken cancellationToken = default);
    Task<bool> HasAircraftAsync(AircraftModelId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(AircraftModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(AircraftModelId id, CancellationToken cancellationToken = default);
}
