using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Interfaces;

public interface IAircraftService
{
    Task<AircraftAggregate> CreateAsync(
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<AircraftAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AircraftAggregate?> GetByRegistrationAsync(string registration, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftAggregate>> GetByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AircraftAggregate> UpdateAsync(
        int id,
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<bool> DeactivateAsync(int id, CancellationToken cancellationToken = default);
}
