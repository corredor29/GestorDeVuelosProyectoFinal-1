using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Interfaces;

public interface IAircraftManufacturersService
{
    Task<AircraftManufacturerAggregate> CreateAsync(string name, string country, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AircraftManufacturerAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AircraftManufacturerAggregate?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetByCountryAsync(string country, CancellationToken cancellationToken = default);
    Task<AircraftManufacturerAggregate> UpdateAsync(int id, string name, string country, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<int> DeleteByCountryAsync(string country, CancellationToken cancellationToken = default);
}
