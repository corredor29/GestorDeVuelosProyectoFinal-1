using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;

public interface IAircraftManufacturersRepository
{
    Task<AircraftManufacturerAggregate?> GetByIdAsync(AircraftManufacturersId id, CancellationToken cancellationToken = default);
    Task<AircraftManufacturerAggregate?> GetByNameAsync(AircraftManufacturersName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetByCountryAsync(AircraftManufacturersCountry country, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(AircraftManufacturersName name, CancellationToken cancellationToken = default);
    Task AddAsync(AircraftManufacturerAggregate aircraftManufacturer, CancellationToken cancellationToken = default);
    Task UpdateAsync(AircraftManufacturerAggregate aircraftManufacturer, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(AircraftManufacturersId id, CancellationToken cancellationToken = default);
    Task<int> DeleteByCountryAsync(AircraftManufacturersCountry country, CancellationToken cancellationToken = default);
    Task<bool> HasAircraftModelsAsync(AircraftManufacturersId id, CancellationToken cancellationToken = default);
}
